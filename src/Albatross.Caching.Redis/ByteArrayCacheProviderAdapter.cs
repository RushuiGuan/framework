﻿using Polly.Caching;
using System.IO;
using System.Text.Json;
using System;

namespace Albatross.Caching.Redis {
	public class ByteArrayCacheProviderAdapter : ICacheProviderAdapter {
		private readonly IAsyncCacheProvider<byte[]> provider;

		public ByteArrayCacheProviderAdapter(IAsyncCacheProvider<byte[]> provider) {
			this.provider = provider;
		}

		public IAsyncCacheProvider<T> Create<T>() {
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
					// do not remove the generic type T here.  The compile will pick the wrong signature and pick Serialize(obj)
					// instead of Serialize<T>(T t).  This makes a difference with T being the base class and the obj being its derived
					// class
					JsonSerializer.Serialize<T>(stream, obj);
					return stream.ToArray();
			}
		}
	}
}
