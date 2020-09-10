using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheManagement {
		string Name { get; }
		ITtlStrategy TtlStrategy { get; }
		string GetCacheKey(Context context);
		Task Init(CancellationToken cancellationToken);
		void Envict(Context context);
		void Register();
		void OnCacheGet(Context context, string cacheKey);
		void OnCacheMiss(Context context, string cacheKey);
		void OnCachePut(Context context, string cacheKey);
		void OnCacheGetError(Context context, string cacheKey, Exception error);
		void OnCachePutError(Context context, string cacheKey, Exception error);
	}
	public interface ICacheManagement<CacheFormat> :ICacheManagement{
	}
}
