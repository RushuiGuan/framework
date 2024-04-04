using Polly.Caching;

namespace Albatross.Caching {
	public interface ICacheProviderAdapter {
		public IAsyncCacheProvider<T> Create<T>();
		public ISyncCacheProvider<T> CreateSync<T>();
	}
}
