using Albatross.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	public abstract class DbSession : DbContext, IDbSession {
		#region constants
		public const string Any = "any";
		public const string EFMigrationHistory = "__EFMigrationsHistory";
		protected readonly ILogger? logger;
		#endregion

		public DbContext DbContext => this;
		public IDbConnection DbConnection => Database.GetDbConnection();
		public List<IDbSessionEventHandler> SessionEventHandlers { get; } = new List<IDbSessionEventHandler>();
		public virtual Assembly[] EntityModelAssemblies => new Assembly[] { GetType().Assembly };
		public virtual string NamespacePrefix => string.Empty;

		public DbSession(DbContextOptions option, ILogger? logger) : base(option) {
			this.SavingChanges += ExecutePriorSaveEventHandlers;
			this.logger = logger;
		}

		private void ExecutePriorSaveEventHandlers(object? sender, SavingChangesEventArgs e) {
			foreach (var item in this.SessionEventHandlers) {
				logger?.LogInformation("Running prior save event handler: {name}", item.GetType().Name);
				item.PriorSave(this);
			}
		}

		protected async Task ExecutePostSaveEventHandlers() {
			foreach (var item in this.SessionEventHandlers) {
				try {
					logger?.LogInformation("Running post save event handler: {name}", item.GetType().Name);
					await item.PostSave();
				} catch (System.Exception ex) {
					logger?.LogError(ex, "Error running PostSave event handler:{class}", item.GetType());
				}
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess) {
			var result = base.SaveChanges(acceptAllChangesOnSuccess);
			ExecutePostSaveEventHandlers().Wait();
			return result;
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
			var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			await ExecutePostSaveEventHandlers();
			return result;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var assembly in EntityModelAssemblies) {
				var items = assembly.GetEntityModels(NamespacePrefix.PostfixIfNotNullOrEmpty('.'));
				foreach (var item in items) {
					item.Build(modelBuilder);
				}
			}
		}
	}
}