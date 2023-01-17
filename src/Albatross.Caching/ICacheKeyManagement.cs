using System.Collections.Generic;

namespace Albatross.Caching {
	public interface ICacheKeyManagement {
		IEnumerable<string> FindKeys(string pattern);
		void FindAndRemoveKeys(string pattern);
		void Remove(IEnumerable<string> keys);
	}
}