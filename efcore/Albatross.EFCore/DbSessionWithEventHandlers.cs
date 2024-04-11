using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public abstract class DbSessionWithEventHandlers : DbSession {
		private readonly IDbEventSessionProvider? eventSessionProvider;

		protected DbSessionWithEventHandlers(DbContextOptions option) : base(option) { }
		protected DbSessionWithEventHandlers(DbContextOptions option, IDbEventSessionProvider eventSessionProvider) : base(option) {
			this.eventSessionProvider = eventSessionProvider;
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess) {
			if (eventSessionProvider == null) {
				return base.SaveChanges(acceptAllChangesOnSuccess);
			} else {
				using (var eventSession = eventSessionProvider.Create()) {
					eventSession.ExecutePriorSaveActions(this).Wait();
					var result = base.SaveChanges(acceptAllChangesOnSuccess);
					eventSession.ExecutePostSaveActions().Wait();
					return result;
				}
			}
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
			if (eventSessionProvider == null) {
				return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			} else {
				using (var eventSession = eventSessionProvider.Create()) {
					await eventSession.ExecutePriorSaveActions(this);
					var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
					await eventSession.ExecutePostSaveActions();
					return result;
				}
			}
		}
	}
}