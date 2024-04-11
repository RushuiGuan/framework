using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public interface IDbSessionEventHandler {
		Task PreSave(IDbSession session);
		Task OnAddedEntry(EntityEntry entry);
		Task OnModifiedEntry(EntityEntry entry);
		Task OnDeletedEntry(EntityEntry entry);
		Task PostSave();
	}
}
