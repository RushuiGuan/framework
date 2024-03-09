using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public abstract class DbSessionWithEventHandlers : DbSession {
		private readonly IDbEventSessionProvider eventSessionProvider;

		public DbSessionWithEventHandlers(DbContextOptions option, IDbEventSessionProvider eventSessionProvider) : base(option) {
			this.eventSessionProvider = eventSessionProvider;
		}
		
		public override int SaveChanges(bool acceptAllChangesOnSuccess) {
			using (var eventSession = eventSessionProvider.Create()) {
				eventSession.ExecutePriorSaveActions(this);
				var result = base.SaveChanges(acceptAllChangesOnSuccess);
				eventSession.ExecutePostSaveActions().Wait();
				return result;
			}
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
			using (var eventSession = eventSessionProvider.Create()) {
				eventSession.ExecutePriorSaveActions(this);
				var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
				await eventSession.ExecutePostSaveActions();
				return result;
			}
		}
	}
}