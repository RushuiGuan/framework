using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Albatross.Caching.EFCore {
	public class AutoCacheEvictionDbSessionEventHander : IDbSessionEventHandler {
	

		public void PriorSave(IDbSession session) {
			foreach(var entry in session.DbContext.ChangeTracker.Entries()) {
				if (entry.State == EntityState.Modified) {
				}else if(entry.State == EntityState.Deleted) {
				} else if (entry.State == EntityState.Added) {
				}
			}
		}

		public Task PostSave() {
			throw new NotImplementedException();
		}
	}
	public class DbSessionWithCachService : DbSession {
		public DbSessionWithCachService(DbContextOptions option, ILogger logger) : base(option, logger) {
		}
	}
}
