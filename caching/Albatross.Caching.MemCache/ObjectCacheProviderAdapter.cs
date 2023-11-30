using Polly.Caching;
using Polly.Caching.Memory;

namespace Albatross.Caching.MemCache {
	public class ObjectCacheProviderAdapter : ICacheProviderAdapter {
		private readonly IAsyncCacheProvider provider;

		public ObjectCacheProviderAdapter(MemoryCacheProvider provider) {
			this.provider = provider;
		}

		public IAsyncCacheProvider<T> Create<T>() {
			return provider.AsyncFor<T>();
		}
	}
}
