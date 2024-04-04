using Polly.Caching;
using System.IO;
using System.Text.Json;
using System;
using Polly.Caching.Distributed;

namespace Albatross.Caching.Redis {
	public class ByteArrayCacheProviderAdapter : ICacheProviderAdapter {
		private readonly NetStandardIDistributedCacheByteArrayProvider cacheProvider;

		public ByteArrayCacheProviderAdapter(NetStandardIDistributedCacheByteArrayProvider cacheProvider) {
			this.cacheProvider = cacheProvider;
		}

		public IAsyncCacheProvider<T> Create<T>() {
			return ((IAsyncCacheProvider<byte[]>)this.cacheProvider).WithSerializer<T, byte[]>(new ByteArrayJsonCacheItemSerializer<T>());
		}

		public ISyncCacheProvider<T> CreateSync<T>() {
			return ((ISyncCacheProvider<byte[]>)this.cacheProvider).WithSerializer<T, byte[]>(new ByteArrayJsonCacheItemSerializer<T>());
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
					JsonSerializer.Serialize<T>(stream, obj);
					return stream.ToArray();
			}
		}
	}
}
