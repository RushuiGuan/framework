using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public interface ICacheKeyManagement {
		Task Init();
		string[] FindKeys(string pattern);
		/// <summary>
		/// the remove operation should be quick and intentionally not async
		/// </summary>
		/// <param name="keys"></param>
		void Remove(params string[] keys);
	}
}