using Polly;
using Polly.Caching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheManagement {
		public const char CacheKeyDelimiter = ':';
		string Name { get; }
		ITtlStrategy TtlStrategy { get; }
		string GetCacheKey(Context context);
		void Register();
		void Remove(params Context[] contexts);
		void Reset();
		
		void OnCacheGet(Context context, string cacheKey);
		void OnCacheMiss(Context context, string cacheKey);
		void OnCachePut(Context context, string cacheKey);
		void OnCacheGetError(Context context, string cacheKey, Exception error);
		void OnCachePutError(Context context, string cacheKey, Exception error);
	}
	public interface ICacheManagement<CacheFormat> : ICacheManagement {
		Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, Context context, CancellationToken cancellationToken);
		Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, Context context);
		Task<(bool, CacheFormat)> TryGetAsync(Context context, CancellationToken cancellationToken);
		Task PutAsync(Context context, CacheFormat value, CancellationToken cancellationToken = default);
	}
}
