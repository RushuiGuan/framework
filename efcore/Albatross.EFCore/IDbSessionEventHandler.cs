using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public interface IDbSessionEventHandler {
		Task PreSave(IDbSession session);
		void OnAddedEntry(EntityEntry entry);
		void OnModifiedEntry(EntityEntry entry);
		void OnDeletedEntry(EntityEntry entry);
		Task PostSave();
	}
}