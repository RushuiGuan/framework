using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public interface ICacheMgmtFactory {
		ICacheManagement Get(string name);
	}
	public class CacheMgmtFactory : ICacheMgmtFactory {
		Dictionary<string, ICacheManagement> dict = new Dictionary<string, ICacheManagement>();

		public CacheMgmtFactory(IEnumerable<ICacheManagement> cacheManagements) {
			foreach(var item in cacheManagements) {
				dict.Add(item.Name, item);
			}
		}
		public ICacheManagement Get(string name) {
			throw new NotImplementedException();
		}
	}
}
