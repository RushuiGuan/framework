using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheKeyManagement {
		IEnumerable<string> FindKeys(string pattern);
		void FindAndRemoveKeys(string pattern);
		void Remove(IEnumerable<string> keys);
	}
}