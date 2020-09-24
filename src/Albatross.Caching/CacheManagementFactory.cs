using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Albatross.Caching {
	public interface ICacheManagementFactory : IReadOnlyDictionary<string, ICacheManagement>{
	}

	public class CacheManagementFactory : Dictionary<string, ICacheManagement>,  ICacheManagementFactory {
		public CacheManagementFactory(IEnumerable<ICacheManagement> cacheManagements) {
			foreach(var item in cacheManagements) {
				Add(item.Name, item);
			}
		}
	}
}
