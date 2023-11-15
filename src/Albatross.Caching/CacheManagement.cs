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
	public abstract class CacheManagement<CacheFormat, KeyFormat> : ICacheManagement<CacheFormat, KeyFormat> where KeyFormat : notnull {
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

		public string Name => this.GetType().Name;
		public virtual string KeyPrefix => this.Name;
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
		public string GetCacheKey(Context context) => context.OperationKey;
		public virtual void BuildKey(KeyBuilder builder, KeyFormat key) {
			builder.Add(this, key);
		}

		public string CreateKey(KeyFormat key, bool postfixWildCard = false) {
			var builder = new KeyBuilder();
			BuildKey(builder, key);
			return builder.Build(postfixWildCard);
		}
		public void Remove(KeyFormat compositeKey) {
			var key = CreateKey(compositeKey, false);
			keyMgmt.Remove(key);
		}
		public void RemoveSelfAndChildren(KeyFormat compositeKey) {
			var key = CreateKey(compositeKey, true);
			var keys = keyMgmt.FindKeys(key);
			keyMgmt.Remove(keys);
		}
		public void Reset() {
			var pattern = new KeyBuilder().BuildCacheResetKeyPattern(this);
			keyMgmt.FindAndRemove(pattern);
		}

		public Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, KeyFormat key, CancellationToken cancellationToken = default) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(Name);
			return policy.ExecuteAsync(func, new Context(CreateKey(key)), cancellationToken);
		}
		public Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, KeyFormat key) {
			var policy = this.registry.Get<IAsyncPolicy<CacheFormat>>(Name);
			return policy.ExecuteAsync(func, new Context(CreateKey(key)));
		}

		public Task<(bool, CacheFormat)> TryGetAsync(KeyFormat key, CancellationToken cancellationToken = default) {
			string keyText = this.CreateKey(key);
			return this.cacheProvider.TryGetAsync(keyText, cancellationToken, false);
		}
		public async Task PutAsync(KeyFormat compositeKey, CacheFormat value, CancellationToken cancellationToken = default) {
			var key = this.CreateKey(compositeKey);
			await this.cacheProvider.PutAsync(key, value, this.TtlStrategy.GetTtl(new Context(key), value), cancellationToken, false);
		}
	}
}