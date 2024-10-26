using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	/// <summary>
	/// scoped instance session class that is created and disposed to handle the 
	/// events when saving changes to the database
	/// </summary>
	public interface IDbEventSession : IDisposable {
		Task ExecutePriorSaveActions(IDbSession session);
		Task ExecutePostSaveActions();
	}
	public class DbEventSession : IDbEventSession {
		bool disposed = false;
		private readonly IServiceScope serviceScope;
		private readonly ILogger logger;
		IEnumerable<IDbSessionEventHandler> changeEventHandlers;

		public DbEventSession(IServiceScope serviceScope) {
			this.serviceScope = serviceScope;
			this.logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<DbEventSession>>();
			this.changeEventHandlers = serviceScope.ServiceProvider.GetService<IEnumerable<IDbSessionEventHandler>>()
				?? Array.Empty<IDbSessionEventHandler>();
		}
		public async Task ExecutePriorSaveActions(IDbSession session) {
			logger.LogDebug("Running dbsession event handlers:\n\t{array}", string.Join("\n\t", changeEventHandlers));
			foreach (var handler in changeEventHandlers) {
				await handler.PreSave(session);
			}
			foreach (var entry in session.DbContext.ChangeTracker.Entries()) {
				switch (entry.State) {
					case EntityState.Added:
						foreach (var handler in changeEventHandlers) {
							handler.OnAddedEntry(entry);
						}
						break;
					case EntityState.Modified:
						foreach (var handler in changeEventHandlers) {
							handler.OnModifiedEntry(entry);
						}
						break;
					case EntityState.Deleted:
						foreach (var handler in changeEventHandlers) {
							handler.OnDeletedEntry(entry);
						}
						break;
				}
			}
		}

		public async Task ExecutePostSaveActions() {
			foreach (var item in changeEventHandlers) {
				try {
					await item.PostSave();
				} catch (System.Exception ex) {
					logger.LogError(ex, "Error running PostSave event handler:{class}", item.ToString());
				}
			}
		}
		public void Dispose() {
			if (!disposed) {
				disposed = true;
				this.serviceScope.Dispose();
			}
		}
	}
}