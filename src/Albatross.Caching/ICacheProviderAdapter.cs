using Polly.Caching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Albatross.Caching {
	public interface ICacheProviderAdapter {
		public IAsyncCacheProvider<T> Create<T>();
	}
}
