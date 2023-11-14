using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public static class CacheManagementExtensions {
		public static ICacheManagement Get(this ICacheManagementFactory factory, string name) {
			if (factory.TryGetValue(name, out ICacheManagement result)) {
				return result;
			} else {
				throw new ArgumentException($"CacheManagement {name} is not registered");
			}
		}
	}
}
