using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Caching {
	public class CacheEvictionService {
		private readonly IServiceProvider serviceProvider;

		public CacheEvictionService(IServiceProvider serviceProvider) {
			this.serviceProvider = serviceProvider;
		}

		public void Evict<Cache, Key>(Key key)  where Cache: ICacheManagement<Key>{
			var cache = serviceProvider.GetRequiredService<Cache>();
			cache.RemoveSelfAndChildren(key);
		}
	}
}
