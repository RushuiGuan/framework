using Polly;
using Polly.Caching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheManagement : ICacheKeyStrategy {
		string Name { get; }
		string KeyPrefix { get; }
		ITtlStrategy TtlStrategy { get; }
		void Register();

		void OnCacheGet(Context context, string cacheKey);
		void OnCacheMiss(Context context, string cacheKey);
		void OnCachePut(Context context, string cacheKey);
		void OnCacheGetError(Context context, string cacheKey, Exception error);
		void OnCachePutError(Context context, string cacheKey, Exception error);
	}

	public interface ICacheKeyBuilder<KeyFormat> {
		void BuildKey(KeyBuilder builder, KeyFormat key);
	}

	public interface ICacheManagement<CacheFormat, KeyFormat> : ICacheManagement, ICacheKeyBuilder<KeyFormat> {
		void Remove(KeyFormat compositeKey);
		void RemoveSelfAndChildren(KeyFormat compositeKey);

		Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, KeyFormat key, CancellationToken cancellationToken);
		Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, KeyFormat key);
		Task<(bool, CacheFormat)> TryGetAsync(KeyFormat key, CancellationToken cancellationToken);
		Task PutAsync(KeyFormat key, CacheFormat value, CancellationToken cancellationToken = default);
	}
}
