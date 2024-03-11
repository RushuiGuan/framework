using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	/// <summary>
	/// a abstract cache class.  The name of the cache is a virtual property with the default implementation 
	/// that returns "Cache-[CacheFormat Class Name]-[KeyFormat Class Name]".  Cache name is used by Polly for 
	/// registration
	/// </summary>
	/// <typeparam name="CacheFormat"></typeparam>
	public abstract class Cache<CacheFormat, KeyFormat> : ICache<CacheFormat, KeyFormat> where KeyFormat : ICacheKey {
		protected readonly ILogger logger;
		protected readonly IAsyncCacheProvider<CacheFormat> cacheProvider;
		private readonly IPolicyRegistry<string> registry;
		private readonly ICacheKeyManagement keyMgmt;

		public Cache(ILogger logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProviderAdapter, ICacheKeyManagement keyMgmt) {
			this.logger = logger;
			this.registry = registry;
			this.keyMgmt = keyMgmt;
			this.cacheProvider = cacheProviderAdapter.Create<CacheFormat>();
			this.Register();
		}

		public virtual string Name => $"Cache-{typeof(CacheFormat).Name}-{typeof(KeyFormat).Name}";
		public abstract ITtlStrategy TtlStrategy { get; }

		// public virtual void OnCacheGet(Context context, string cacheKey) { }
		public virtual void OnCacheMiss(Context context, string cacheKey) => logger.LogDebug("cache miss: {key}", cacheKey);
		public virtual void OnCachePut(Context context, string cacheKey) => logger.LogDebug("cache put: {key}", cacheKey);
		public virtual void OnCacheGetError(Context context, string cacheKey, Exception error) => logger.LogError(error, "Error getting cache {name}", cacheKey);
		public virtual void OnCachePutError(Context context, string cacheKey, Exception error) => logger.LogError(error, "Error putting cache {name}", cacheKey);

		public void Register() {
			if (!registry.ContainsKey(Name)) {
				logger.LogInformation("Register Cache Management {cacheName}", Name);
				var policy = Policy.CacheAsync<CacheFormat>(cacheProvider, TtlStrategy, this,
					null, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
				registry.Add(Name, policy);
			} else {
				logger.LogError("Cache {name} has already been registered", Name);
			}
		}
		/// <summary>
		/// Implements <see cref="ICacheKeyStrategy.GetCacheKey(Context)"/> to return the <see cref="Context.OperationKey"/>."/>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public string GetCacheKey(Context context) => context.OperationKey;

		public Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, KeyFormat keyValue, CancellationToken cancellationToken = default) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(Name);
			return policy.ExecuteAsync(func, new Context(keyValue.Key), cancellationToken);
		}
		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, KeyFormat keyValue) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(Name);
			return policy.ExecuteAsync(func, new Context(keyValue.Key));
		}

		public Task<(bool, CacheFormat)> TryGetAsync(KeyFormat keyValue, CancellationToken cancellationToken = default) {
			return this.cacheProvider.TryGetAsync(keyValue.Key, cancellationToken, false);
		}
		public async Task PutAsync(KeyFormat keyValue, CacheFormat cacheValue, CancellationToken cancellationToken = default) {
			await this.cacheProvider.PutAsync(keyValue.Key, cacheValue, this.TtlStrategy.GetTtl(new Context(keyValue.Key), cacheValue), cancellationToken, false);
		}
	}
}