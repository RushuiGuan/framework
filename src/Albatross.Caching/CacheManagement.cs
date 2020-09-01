using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching {
	public abstract class CacheManagement<CacheFormat> : ICacheManagement<CacheFormat>, ICacheKeyStrategy {
		private readonly ILogger logger;

		public CacheManagement(ILogger logger) {
			this.logger = logger;
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

		public void Register(IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider) {
			var policy = Policy.CacheAsync<CacheFormat>(cacheProvider.AsyncFor<CacheFormat>(), TtlStrategy, this,
				OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
			registry.Add(Name, policy);
		}

		public virtual string GetCacheKey(Context context) {
			return $"{Name}-{context.OperationKey}";
		}
	}
}
