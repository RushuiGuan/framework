using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Albatross.Caching {
	public interface ICacheManagementFactory {
		ICacheManagement Get(string cacheName);
	}

	public class CacheManagementFactory : ICacheManagementFactory {
		Dictionary<string, ICacheManagement> registry = new Dictionary<string, ICacheManagement>();

		public CacheManagementFactory(IEnumerable<ICacheManagement> cacheManagements) {
			foreach(var item in cacheManagements) {
				registry.Add(item.Name, item);
			}
		}
		public ICacheManagement Get(string cacheName) {
			return registry[cacheName];
		}
	}
}
