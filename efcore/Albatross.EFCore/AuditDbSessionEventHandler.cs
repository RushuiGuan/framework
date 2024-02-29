using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public class AuditDbSessionEventHandler<T> : IDbSessionEventHandler where T : class {
		public const string NotAvailable = "N/A";
		public string? User { get; }
		public Task PostSave() => Task.CompletedTask;

		public AuditDbSessionEventHandler() : this(null) { }
		public AuditDbSessionEventHandler(string? user) {
			User = user;
		}
		public void PriorSave(IDbSession session) {
			foreach (var entry in session.DbContext.ChangeTracker.Entries<T>()) {
				if (entry.State == EntityState.Modified) {
					if (entry.Entity is IModifiedBy audit1) { audit1.ModifiedBy = this.User ?? NotAvailable; }
					if (entry.Entity is IModifiedUtc audit2) { audit2.ModifiedUtc = DateTime.UtcNow; }
				} else if (entry.State == EntityState.Added) {
					if (entry.Entity is ICreatedBy audit1) { audit1.CreatedBy = this.User ?? NotAvailable; }
					if (entry.Entity is IModifiedBy audit2) { audit2.ModifiedBy = this.User ?? NotAvailable; }
					var utcNow = DateTime.UtcNow;
					if (entry.Entity is ICreatedUtc audit3) { audit3.CreatedUtc = utcNow; }
					if (entry.Entity is IModifiedUtc audit4) { audit4.ModifiedUtc = utcNow; }
				}
			}
		}
	}
}