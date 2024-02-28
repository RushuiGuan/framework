using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Caching.EFCore {
	public class DbSessionWithCachService : DbSession {
		public DbSessionWithCachService(DbContextOptions option) : base(option) {
		}
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
