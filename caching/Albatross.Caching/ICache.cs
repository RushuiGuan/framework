using Polly;
using Polly.Caching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICache : ICacheKeyStrategy {
		string AsyncName { get; }
		ITtlStrategy TtlStrategy { get; }
	}

	public interface ICache<CacheFormat, KeyFormat> : ICache where KeyFormat : ICacheKey {
		Task<CacheFormat> ExecuteAsync(Func<Context, CancellationToken, Task<CacheFormat>> func, KeyFormat key, CancellationToken cancellationToken);
		Task<CacheFormat> ExecuteAsync(Func<Context, Task<CacheFormat>> func, KeyFormat key);
		Task<(bool, CacheFormat)> TryGetAsync(KeyFormat key, CancellationToken cancellationToken);
		Task PutAsync(KeyFormat key, CacheFormat value, CancellationToken cancellationToken = default);


		CacheFormat Execute(Func<Context, CacheFormat> func, KeyFormat keyValue);
		(bool, CacheFormat) TryGet(KeyFormat keyValue);
		void Put(KeyFormat keyValue, CacheFormat cacheValue);
	}
}