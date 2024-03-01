using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Albatross.EFCore.AutoCacheEviction {
	public class DbSessionWithAutoCachEviction : DbSession {
		public DbSessionWithAutoCachEviction(DbContextOptions option, ILogger logger) : base(option, logger) {
		}
	}
}
