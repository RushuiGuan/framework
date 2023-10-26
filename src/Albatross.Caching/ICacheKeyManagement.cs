using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheKeyManagement {
		Task<IEnumerable<string>> FindKeys(string pattern);
		Task FindAndRemoveKeys(string pattern);
		void Remove(IEnumerable<string> keys);
	}
}