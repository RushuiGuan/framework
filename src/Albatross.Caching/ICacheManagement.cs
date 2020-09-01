using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public interface ICacheManagement {
		string Name { get; }
		ITtlStrategy TtlStrategy { get; }
		string GetCacheKey(Context context);
		void Register(IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider);
		void OnCacheGet(Context context, string cacheKey);
		void OnCacheMiss(Context context, string cacheKey);
		void OnCachePut(Context context, string cacheKey);
		void OnCacheGetError(Context context, string cacheKey, Exception error);
		void OnCachePutError(Context context, string cacheKey, Exception error);
	}
	public interface ICacheManagement<CacheFormat> :ICacheManagement{
	}
}
