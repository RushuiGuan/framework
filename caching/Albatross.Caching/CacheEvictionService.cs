using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public class CacheEvictionService {
		private readonly IServiceProvider serviceProvider;

		public CacheEvictionService(IServiceProvider serviceProvider) {
			this.serviceProvider = serviceProvider;
		}

		public void Evict<T, K>(ICachedObject<T, K> cachedObject) where T : ICacheManagement<K> {
			var cacheMgmt = this.serviceProvider.GetRequiredService<T>();
			cacheMgmt.RemoveSelfAndChildren(cachedObject.GetCacheKey());
		}
	}
}
