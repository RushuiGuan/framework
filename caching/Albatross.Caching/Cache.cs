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
		protected readonly IAsyncCacheProvider<CacheFormat> asyncCacheProvider;
		protected readonly ISyncCacheProvider<CacheFormat> syncCacheProvider;
		private readonly IPolicyRegistry<string> registry;

		public Cache(ILogger logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProviderAdapter) {
			this.logger = logger;
			this.registry = registry;
			this.asyncCacheProvider = cacheProviderAdapter.Create<CacheFormat>();
			this.syncCacheProvider = cacheProviderAdapter.CreateSync<CacheFormat>();
			this.Register();
		}

		public string AsyncName => $"async-cache-{typeof(CacheFormat).Name}-{typeof(KeyFormat).Name}";
		public string SyncName => $"sync-cache-{typeof(CacheFormat).Name}-{typeof(KeyFormat).Name}";
		public abstract ITtlStrategy TtlStrategy { get; }

		public void OnCacheGet(Context context, string cacheKey) { }
		public void OnCacheMiss(Context context, string cacheKey) => logger.LogDebug("cache miss: {key}", cacheKey);
		public void OnCachePut(Context context, string cacheKey) => logger.LogDebug("cache put: {key}", cacheKey);
		public void OnCacheGetError(Context context, string cacheKey, Exception error) => logger.LogError(error, "Error getting cache {name}", cacheKey);
		public void OnCachePutError(Context context, string cacheKey, Exception error) => logger.LogError(error, "Error putting cache {name}", cacheKey);

		public void Register() {
			if (!registry.ContainsKey(AsyncName)) {
				logger.LogInformation("Register Cache Management {async}, {sync}", AsyncName, SyncName);
				var asyncPolicy = Policy.CacheAsync<CacheFormat>(asyncCacheProvider, TtlStrategy, this,
					OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
				registry.Add(AsyncName, asyncPolicy);
				var syncPolicy = Policy.Cache<CacheFormat>(syncCacheProvider, TtlStrategy, this,
					OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
				registry.Add(SyncName, syncPolicy);
			} else {
				logger.LogError("Cache {name} has already been registered", AsyncName);
			}
		}
		/// <summary>
		/// Implements <see cref="ICacheKeyStrategy.GetCacheKey(Context)"/> to return the <see cref="Context.OperationKey"/>."/>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public string GetCacheKey(Context context) => context.OperationKey;


		public Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, KeyFormat keyValue, CancellationToken cancellationToken = default) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(AsyncName);
			return policy.ExecuteAsync(func, new Context(keyValue.Key), cancellationToken);
		}
		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, KeyFormat keyValue) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(AsyncName);
			return policy.ExecuteAsync(func, new Context(keyValue.Key));
		}

		public Task<(bool, CacheFormat)> TryGetAsync(KeyFormat keyValue, CancellationToken cancellationToken = default) {
			return this.asyncCacheProvider.TryGetAsync(keyValue.Key, cancellationToken, false);
		}
		public async Task PutAsync(KeyFormat keyValue, CacheFormat cacheValue, CancellationToken cancellationToken = default) {
			await this.asyncCacheProvider.PutAsync(keyValue.Key, cacheValue, this.TtlStrategy.GetTtl(new Context(keyValue.Key), cacheValue), cancellationToken, false);
		}

		public CacheFormat Execute(Func<Context, CacheFormat> func, KeyFormat keyValue) {
			var policy = this.registry.Get<ISyncPolicy<CacheFormat>>(SyncName);
			return policy.Execute(func, new Context(keyValue.Key));
		}
		public (bool, CacheFormat) TryGet(KeyFormat keyValue) => this.syncCacheProvider.TryGet(keyValue.Key);
		public void Put(KeyFormat keyValue, CacheFormat cacheValue)
			=> this.syncCacheProvider.Put(keyValue.Key, cacheValue, this.TtlStrategy.GetTtl(new Context(keyValue.Key), cacheValue));
	}
}