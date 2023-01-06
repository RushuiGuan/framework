using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Linq;
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
		private readonly IRedisKeyManagement keyMgmt;
		public const string Context_Init = "init";

		public CacheManagement(ILogger logger, IPolicyRegistry<string> registry, IAsyncCacheProviderConverter converter, IRedisKeyManagement keyMgmt) {
			this.logger = logger;
			this.registry = registry;
			this.keyMgmt = keyMgmt;
			this.cacheProvider = converter.Get<CacheFormat>();
		}

		public virtual string Name => this.GetType().Name;
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
			return $"{Name}:{context.OperationKey}".ToLowerInvariant();
		}

		public void Remove(params Context[] contexts) {
			var keys = contexts.Select(args => GetCacheKey(args));
			logger.LogInformation("Removing cache: {@key}", keys);
			keyMgmt.Remove(keys.ToArray());
		}

		public void Reset() {
			var pattern = GetCacheKey(new Context()) + "*";
			this.keyMgmt.FindAndRemoveKeys(pattern);
		}

		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, Context context) {
			var policy = this.registry.GetAsyncPolicy<CacheFormat>(Name);
			return policy.ExecuteAsync(func, context);
		}
	}
}
