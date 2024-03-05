using Albatross.Authentication;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.Audit {
	public class AuditDbSessionEventHandler : IDbChangeEventHandler {
		public string User { get; }

		public bool HasPostSaveOperation => false;
		public Task PostSave() => Task.CompletedTask;

		public AuditDbSessionEventHandler(IGetCurrentUser getCurrentUser) {
			User = getCurrentUser.Get();
		}
		public void OnAddedEntry(EntityEntry entry) {
			if (entry.Entity is ICreatedBy audit1) { audit1.CreatedBy = this.User; }
			if (entry.Entity is IModifiedBy audit2) { audit2.ModifiedBy = this.User; }
			var utcNow = DateTime.UtcNow;
			if (entry.Entity is ICreatedUtc audit3) { audit3.CreatedUtc = utcNow; }
			if (entry.Entity is IModifiedUtc audit4) { audit4.ModifiedUtc = utcNow; }
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (entry.Entity is IModifiedBy audit1) { audit1.ModifiedBy = this.User; }
			if (entry.Entity is IModifiedUtc audit2) { audit2.ModifiedUtc = DateTime.UtcNow; }
		}
		public void OnDeletedEntry(EntityEntry entry) { }
	}
}