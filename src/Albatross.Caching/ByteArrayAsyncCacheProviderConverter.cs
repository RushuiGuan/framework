using Polly.Caching;
using System.IO;
using System.Text.Json;
using System;

namespace Albatross.Caching {
	public class ByteArrayAsyncCacheProviderConverter : IAsyncCacheProviderConverter {
		private readonly IAsyncCacheProvider<byte[]> provider;

		public ByteArrayAsyncCacheProviderConverter(IAsyncCacheProvider<byte[]> provider) {
			this.provider = provider;
		}

		public IAsyncCacheProvider<T> Get<T>() {
			return this.provider.WithSerializer<T, byte[]>(new ByteArrayJsonCacheItemSerializer<T>());
		}
	}
	public class ByteArrayJsonCacheItemSerializer<T> : ICacheItemSerializer<T, byte[]> {
		public T Deserialize(byte[] bytes) {
			switch (bytes) {
				case T data:
					return data;
				default:
					return JsonSerializer.Deserialize<T>(new MemoryStream(bytes))
						?? throw new InvalidOperationException();
			}
		}

		public byte[] Serialize(T obj) {
			switch (obj) {
				case byte[] bytes:
					return bytes; 
				default:
					var stream = new MemoryStream();
					JsonSerializer.Serialize(stream, obj);
					return stream.ToArray();
			}
		}
	}
}
