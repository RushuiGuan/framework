using System.Threading.Tasks;

namespace Albatross.EFCore {
	public interface IDbSessionEventHandler{
		void PriorSave(IDbSession session);
		Task PostSave();
	}
}
