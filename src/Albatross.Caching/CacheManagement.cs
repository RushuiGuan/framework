using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public abstract class CacheManagement<CacheFormat> : ICacheManagement<CacheFormat>, ICacheKeyStrategy {
		protected readonly ILogger logger;
		protected readonly IAsyncCacheProvider<CacheFormat> cacheProvider;
		private readonly IPolicyRegistry<string> registry;
		private readonly IMemoryCache cache;

		public CacheManagement(ILogger logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCache cache) {
			this.logger = logger;
			this.registry = registry;
			this.cacheProvider = cacheProvider.AsyncFor<CacheFormat>();
			this.cache = cache;
		}

		public abstract string Name { get; }
		public abstract ITtlStrategy TtlStrategy { get; }

		public virtual void OnCacheGet(Context context, string cacheKey) { }
		public virtual void OnCacheMiss(Context context, string cacheKey) {
			logger.LogInformation("Cache Miss {name}: {key}", Name, cacheKey);
		}

		public virtual void OnCachePut(Context context, string cacheKey) {
			logger.LogInformation("Cache Put {name}: {key}", Name, cacheKey);
		}
		public virtual void OnCacheGetError(Context context, string cacheKey, Exception error) { }
		public virtual void OnCachePutError(Context context, string cacheKey, Exception error) { }

		public void Register() {
			var policy = Policy.CacheAsync<CacheFormat>(cacheProvider, TtlStrategy, this,
				OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
			registry.Add(Name, policy);
		}

		public virtual string GetCacheKey(Context context) {
			return $"{Name}-{context.OperationKey}";
		}

		public virtual void Envict(Context context) {
			string key = GetCacheKey(context);
			cache.Remove(key);
		}

		public virtual Task Init(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}
	}
}
