using Polly.Caching;

namespace Albatross.Caching.Redis {
	public class ObjectCacheProviderAdapter : ICacheProviderAdapter {
		private readonly IAsyncCacheProvider provider;

		public ObjectCacheProviderAdapter(IAsyncCacheProvider provider) {
			this.provider = provider;
		}

		public IAsyncCacheProvider<T> Create<T>() {
			return provider.AsyncFor<T>();
		}
	}
}
