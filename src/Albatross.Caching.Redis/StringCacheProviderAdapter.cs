using Polly.Caching;
using System.Text.Json;
using System;

namespace Albatross.Caching.Redis {
	public class StringCacheProviderAdapter : ICacheProviderAdapter {
		private readonly IAsyncCacheProvider<string> cacheProvider;

		public StringCacheProviderAdapter(IAsyncCacheProvider<string> cacheProvider) {
			this.cacheProvider = cacheProvider;
		}

		public IAsyncCacheProvider<T> Create<T>() {
			return this.cacheProvider.WithSerializer<T, string>(new StringJsonCacheItemSerializer<T>());
		}
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
					// do not remove the generic type T here.  The compile will pick the wrong signature and pick Serialize(obj)
					// instead of Serialize<T>(T t).  This makes a difference with T being the base class and the obj being its derived
					// class
					return JsonSerializer.Serialize<T>(obj);
			}
		}
	}
}
