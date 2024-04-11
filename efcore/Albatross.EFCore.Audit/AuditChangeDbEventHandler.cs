using Albatross.Authentication;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.Audit {
	public class AuditChangeDbEventHandler : IDbSessionEventHandler {
		private readonly IGetCurrentUser getCurrentUser;
		public Task PreSave(IDbSession session) => Task.CompletedTask;
		public Task PostSave() => Task.CompletedTask;
		public override string ToString() => nameof(AuditChangeDbEventHandler);
		public AuditChangeDbEventHandler(IGetCurrentUser getCurrentUser) {
			this.getCurrentUser = getCurrentUser;
		}
		public void OnAddedEntry(EntityEntry entry) {
			if (entry.Entity is ICreatedBy audit1) { audit1.CreatedBy = this.getCurrentUser.Get(); }
			if (entry.Entity is IModifiedBy audit2) { audit2.ModifiedBy = this.getCurrentUser.Get(); }
			var utcNow = DateTime.UtcNow;
			if (entry.Entity is ICreatedUtc audit3) { audit3.CreatedUtc = utcNow; }
			if (entry.Entity is IModifiedUtc audit4) { audit4.ModifiedUtc = utcNow; }
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (entry.Entity is IModifiedBy audit1) { audit1.ModifiedBy = this.getCurrentUser.Get(); }
			if (entry.Entity is IModifiedUtc audit2) { audit2.ModifiedUtc = DateTime.UtcNow; }
		}
		public void OnDeletedEntry(EntityEntry entry) { }
	}
}