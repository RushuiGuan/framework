using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public class AuditDbSessionEventHandler<T> : IDbSessionEventHandler where T : class {
		public AuditDbSessionEventHandler() { }
		public AuditDbSessionEventHandler(string? user) {
			User = user;
		}

		public string? User { get; }
		public Task PostSave() => Task.CompletedTask;

		public void PriorSave(IDbSession session) {
			foreach (var entry in session.DbContext.ChangeTracker.Entries<T>()) {
				if (entry.State == EntityState.Modified) {
					if (entry.Entity is IModifiedBy audit1 && !string.IsNullOrEmpty(User)) { 
						audit1.ModifiedBy = this.User; 
					}
					if (entry.Entity is IModifiedUtc audit2) { 
						audit2.ModifiedUtc = DateTime.UtcNow; 
					}
				} else if (entry.State == EntityState.Added) {
					if (entry.Entity is ICreatedBy audit1 && !string.IsNullOrEmpty(this.User)) { 
						audit1.CreatedBy = this.User; 
					}
					if (entry.Entity is ICreatedUtc audit2) { 
						audit2.CreatedUtc = DateTime.UtcNow; 
					}
				}
			}
		}
	}
}
