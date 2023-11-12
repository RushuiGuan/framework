using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
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

		public CacheManagement(ILogger logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProviderAdapter, ICacheKeyManagement keyMgmt) {
			this.logger = logger;
			this.registry = registry;
			this.keyMgmt = keyMgmt;
			this.cacheProvider = cacheProviderAdapter.Create<CacheFormat>();
		}

		public virtual string Name => this.GetType().Name;
		public abstract ITtlStrategy TtlStrategy { get; }

		public virtual void OnCacheGet(Context context, string cacheKey) { }
		public virtual void OnCacheMiss(Context context, string cacheKey) => logger.LogInformation("Cache miss: {key}", cacheKey);
		public virtual void OnCachePut(Context context, string cacheKey) => logger.LogInformation("Cache put: {key}", cacheKey);
		public virtual void OnCacheGetError(Context context, string cacheKey, Exception error) => logger.LogError(error, "Error getting cache {name}", this.Name);
		public virtual void OnCachePutError(Context context, string cacheKey, Exception error) => logger.LogError(error, "Error putting cache {name}", this.Name);

		public void Register() {
			if (!registry.ContainsKey(Name)) {
				var policy = Policy.CacheAsync<CacheFormat>(cacheProvider, TtlStrategy, this,
					OnCacheGet, OnCacheMiss, OnCachePut, OnCacheGetError, OnCachePutError);
				registry.Add(Name, policy);
			} else {
				logger.LogError("CacheManagement {name} has already been registered", Name);
			}
		}

		public virtual string GetCacheKey(Context context) => new CompositeKeyBuilder(this).Add(context.OperationKey).Build(false);

		public void Remove(params object[] compositeKey) {
			var key = new CompositeKeyBuilder(this).Add(compositeKey).Build(false);
			if (keyMgmt.IsPattern(key)) {
				var keys = keyMgmt.FindKeys(key);
				keyMgmt.Remove(keys);
			} else {
				keyMgmt.Remove(key);
			}
		}
		public void Reset() {
			var key = new CompositeKeyBuilder(this).Build(true);
			var keys = keyMgmt.FindKeys(key);
			keyMgmt.Remove(keys);
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