using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheKeyManagement {
		/// <summary>
		/// key seeking is not a recommended operation in a caching setup.  should avoid if possible
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		Task<IEnumerable<string>> FindKeys(string pattern);
		Task FindAndRemoveKeys(string pattern);

		/// <summary>
		/// the remove operation should be quick and intentionally not async
		/// </summary>
		/// <param name="keys"></param>
		void Remove(IEnumerable<string> keys);
	}
}