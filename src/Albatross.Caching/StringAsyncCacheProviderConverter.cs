using Polly.Caching;
using System.Text.Json;
using System;

namespace Albatross.Caching {
	public class StringAsyncCacheProviderConverter : IAsyncCacheProviderConverter {
		private readonly IAsyncCacheProvider<string> cacheProvider;

		public StringAsyncCacheProviderConverter(IAsyncCacheProvider<string> cacheProvider) {
			this.cacheProvider = cacheProvider;
		}

		public IAsyncCacheProvider<T> Get<T>() {
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
					return JsonSerializer.Serialize(obj);
			}
		}
	}
}
