﻿using Microsoft.Extensions.Caching.Memory;
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
		public const string Context_Init = "init";

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
			if (string.IsNullOrEmpty(context.OperationKey)) {
				return Name;
			} else {
				return $"{Name}-{context.OperationKey}";
			}
		}

		public void Evict(params Context[] contexts) {
			foreach (var context in contexts) {
				string key = GetCacheKey(context);
				logger.LogInformation("Evicting cache: {key}", key);
				this.cache.Remove(key);
			}
		}

		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, Context context) {
			return this.registry.GetAsyncPolicy<CacheFormat>(Name).ExecuteAsync(func, context);
		}
	}
}
