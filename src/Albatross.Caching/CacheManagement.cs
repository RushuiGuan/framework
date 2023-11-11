using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {

	/// <summary>
	/// a abstract cache management class.  All cache created by this class should have a prefix of $"{Name}:".  The name of the cache management is a virtual property with
	/// the default implementation that returns the class name.
	/// </summary>
	/// <typeparam name="CacheFormat"></typeparam>
	public abstract class CacheManagement<CacheFormat> : ICacheManagement<CacheFormat>, ICacheKeyStrategy {
		protected readonly ILogger logger;
		protected readonly IAsyncCacheProvider<CacheFormat> cacheProvider;
		private readonly IPolicyRegistry<string> registry;
		private readonly ICacheKeyManagement keyMgmt;
		public const string Context_Init = "init";

		public CacheManagement(ILogger logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProviderAdapter, ICacheKeyManagement keyMgmt) {
			this.logger = logger;
			this.registry = registry;
			this.keyMgmt = keyMgmt;
			this.cacheProvider = cacheProviderAdapter.Create<CacheFormat>();
		}

		public virtual string Name => this.GetType().Name;
		public abstract ITtlStrategy TtlStrategy { get; }

		public virtual void OnCacheGet(Context context, string cacheKey) { }
		public virtual void OnCacheMiss(Context context, string cacheKey) {
			if (!context.ContainsKey(Context_Init)) {
				logger.LogInformation("Cache miss: {key}", cacheKey);
			}
		}
		public virtual void OnCachePut(Context context, string cacheKey) {
			if (!context.ContainsKey(Context_Init)) {
				logger.LogInformation("Cache put: {key}", cacheKey);
			}
		}
		public virtual void OnCacheGetError(Context context, string cacheKey, Exception error) {
			logger.LogError(error, "Cache get error for {name}", this.Name);
		}
		public virtual void OnCachePutError(Context context, string cacheKey, Exception error) {
			logger.LogError(error, "Cache put error for {name}", this.Name);

		}

		public void Register() {
			if (!registry.ContainsKey(Name)) {
				var policy = Policy.CacheAsync<CacheFormat>(cacheProvider, TtlStrategy, this,
					OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
				registry.Add(Name, policy);
			} else {
				logger.LogError("CacheManagement {cacheName} has already been registered", Name);
			}
		}

		// dash below is intentional so that it can be used as part of the prefix to evict all cache created by this class
		public virtual string GetCacheKey(Context context) => $"{Name}{ICacheManagement.CacheKeyDelimiter}{context.OperationKey}".ToLowerInvariant();

		public void Remove(params Context[] contexts) {
			var keys = contexts.Select(args => GetCacheKey(args));
			logger.LogInformation("Removing cache: {@key}", keys);
			keyMgmt.Remove(keys.ToArray());
		}

		public ValueTask Reset() {
			var pattern = GetCacheKey(new Context()) + "*";
			return this.keyMgmt.FindAndRemoveKeys(pattern);
		}

		public Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, Context context, CancellationToken cancellationToken) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(Name);
			return policy.ExecuteAsync(func, context, cancellationToken);
		}

		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, Context context) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(Name);
			return policy.ExecuteAsync(func, context);
		}

		public Task<(bool, CacheFormat)> TryGetAsync(Context context, CancellationToken cancellationToken) {
			string key = GetCacheKey(context);
			return this.cacheProvider.TryGetAsync(key, cancellationToken, false);
		}

		public Task PutAsync(Context context, CacheFormat value, CancellationToken cancellationToken = default) =>
			this.cacheProvider.PutAsync(GetCacheKey(context), value, this.TtlStrategy.GetTtl(context, value), cancellationToken, false);
	}
}