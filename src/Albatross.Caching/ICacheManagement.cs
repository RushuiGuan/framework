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
	public interface ICacheManagement<CacheFormat, KeyFormat> : ICacheManagement {
		string BuildKey(KeyFormat key);
		void Remove(KeyFormat compositeKey);
		void RemoveAll(KeyFormat compositeKey);

		Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, Context context, CancellationToken cancellationToken);
		Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, Context context);
		Task<(bool, CacheFormat)> TryGetAsync(KeyFormat key, CancellationToken cancellationToken);
		Task PutAsync(KeyFormat key, CacheFormat value, CancellationToken cancellationToken = default);
	}
}
