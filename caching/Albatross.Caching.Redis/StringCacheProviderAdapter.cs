using Polly.Caching;
using System.Text.Json;
using System;
using Polly.Caching.Distributed;

namespace Albatross.Caching.Redis {
	public class StringCacheProviderAdapter : ICacheProviderAdapter {
		private readonly NetStandardIDistributedCacheStringProvider cacheProvider;

		public StringCacheProviderAdapter(NetStandardIDistributedCacheStringProvider cacheProvider) {
			this.cacheProvider = cacheProvider;
		}

		public IAsyncCacheProvider<T> Create<T>() 
			=> ((IAsyncCacheProvider<string>)this.cacheProvider).WithSerializer(new StringJsonCacheItemSerializer<T>());

		public ISyncCacheProvider<T> CreateSync<T>()
			=> ((ISyncCacheProvider<string>)this.cacheProvider).WithSerializer<T, string>(new StringJsonCacheItemSerializer<T>());
			
	}
	public class StringJsonCacheItemSerializer<T> : ICacheItemSerializer<T, string> {
		public T Deserialize(string text) {
			switch (text) {
				case T data:
					return data;
				default:
					return JsonSerializer.Deserialize<T>(text)
						?? throw new InvalidOperationException();
			}
		}

		public string Serialize(T obj) {
			switch (obj) {
				case string text:
					return text;
				default:
					return JsonSerializer.Serialize<T>(obj);
			}
		}
	}
}
