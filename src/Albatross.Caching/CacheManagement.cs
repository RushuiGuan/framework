using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public abstract class CacheManagement<CacheFormat> : ICacheManagement<CacheFormat>, ICacheKeyStrategy {
		protected readonly ILogger logger;
		protected readonly IAsyncCacheProvider<CacheFormat> cacheProvider;
		private readonly IPolicyRegistry<string> registry;
		private readonly IMemoryCacheExtended cache;
		public const string Context_Init = "init";

		public CacheManagement(ILogger logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCacheExtended cache) {
			this.logger = logger;
			this.registry = registry;
			this.cacheProvider = cacheProvider.AsyncFor<CacheFormat>();
			this.cache = cache;
		}

		public abstract string Name { get; }
		public abstract ITtlStrategy TtlStrategy { get; }

		public virtual void OnCacheGet(Context context, string cacheKey) {
		}
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
		public virtual void OnCacheGetError(Context context, string cacheKey, Exception error) { }
		public virtual void OnCachePutError(Context context, string cacheKey, Exception error) { }

		public void Register() {
			if (!registry.ContainsKey(Name)) {
				var policy = Policy.CacheAsync<CacheFormat>(cacheProvider, TtlStrategy, this,
					OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
				registry.Add(Name, policy);
			} else {
				logger.LogError("CacheManagement {cacheName} has already been registered", Name);
			}
		}

		public virtual string GetCacheKey(Context context) {
			// dash below is intentional so that it can be used as part of the prefix to evict all cache created by this class
			return $"{Name}-{context.OperationKey}".ToLowerInvariant();
		}

		public void Evict(params Context[] contexts) {
			var keys = contexts.Select(args => GetCacheKey(args));
			logger.LogInformation("Evicting cache: {@key}", keys);
			this.cache.Envict(keys);
		}

		public void EvictAll() {
			var prefix = GetCacheKey(new Context());
			var keys = this.cache.Keys.Select(args => args.ToString() ?? String.Empty)
				.Where(args => args.StartsWith(prefix)).ToArray();
			this.cache.Envict(keys);
		}

		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, Context context) {
			return this.registry.GetAsyncPolicy<CacheFormat>(Name).ExecuteAsync(func, context);
		}
	}
}
