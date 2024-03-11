using Polly;
using Polly.Caching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICache : ICacheKeyStrategy {
		string Name { get; }
		ITtlStrategy TtlStrategy { get; }
		// void Register();
		//void OnCacheGet(Context context, string cacheKey);
		//void OnCacheMiss(Context context, string cacheKey);
		//void OnCachePut(Context context, string cacheKey);
		//void OnCacheGetError(Context context, string cacheKey, Exception error);
		//void OnCachePutError(Context context, string cacheKey, Exception error);
	}

	public interface ICache<CacheFormat, KeyFormat> : ICache where KeyFormat : ICacheKey {
		Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, KeyFormat key, CancellationToken cancellationToken);
		Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, KeyFormat key);
		Task<(bool, CacheFormat)> TryGetAsync(KeyFormat key, CancellationToken cancellationToken);
		Task PutAsync(KeyFormat key, CacheFormat value, CancellationToken cancellationToken = default);
	}
}
