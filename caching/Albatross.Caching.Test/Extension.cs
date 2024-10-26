using Albatross.Hosting.Test;
using System;

namespace Albatross.Caching.Test {
	public static class Extension {
		public static TestHost GetTestHost(this string hostType) {
			if (hostType == MemCacheHost.HostType) {
				return new MemCacheHost();
			} else if (hostType == RedisCacheHost.HostType) {
				return new RedisCacheHost();
			} else {
				throw new NotSupportedException();
			}
		}
	}
}