using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public interface IDbChangeEventHandler {
		void OnAddedEntry(EntityEntry entry);
		void OnModifiedEntry(EntityEntry entry);
		void OnDeletedEntry(EntityEntry entry);
		bool HasPostSaveOperation { get; }
		Task PostSave();
	}
}
