using Albatross.Collections;
using Albatross.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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
		private readonly IDbChangeEventHandlerFactory changeEventHandlerFactory;
		protected readonly ILogger? logger;
		#endregion

		public virtual bool UseChangeEventHandler => false;
		public DbContext DbContext => this;
		public IDbConnection DbConnection => Database.GetDbConnection();
		public virtual Assembly[] EntityModelAssemblies => new Assembly[] { GetType().Assembly };
		public virtual string NamespacePrefix => string.Empty;

		public DbSession(DbContextOptions option, IDbChangeEventHandlerFactory changeEventHandlerFactory, ILogger? logger) : base(option) {
			if (UseChangeEventHandler) {
				this.SavingChanges += ExecutePriorSaveEventHandlers;
			}

			this.changeEventHandlerFactory = changeEventHandlerFactory;
			this.logger = logger;
		}

		protected void ExecutePriorSaveEventHandlers(object? sender, SavingChangesEventArgs e) {
			var handlers = this.changeEventHandlerFactory.Create();
			foreach (var entry in this.ChangeTracker.Entries()) {
				switch (entry.State) {
					case EntityState.Added:
						handlers.ForEach(x=>x.OnAddedEntry(entry));
						break;
					case EntityState.Modified:
						handlers.ForEach(x=>x.OnModifiedEntry(entry));
						break;
					case EntityState.Deleted:
						handlers.ForEach(x=>x.OnDeletedEntry(entry));
						break;
				}
			}
		}

		protected async Task ExecutePostSaveEventHandlers() {
			var handlers = this.changeEventHandlerFactory.Create();
			foreach (var item in handlers) {
				if (item.HasPostSaveOperation) {
					try {
						logger?.LogInformation("Running post save event handler: {name}", item.GetType().Name);
						await item.PostSave();
					} catch (System.Exception ex) {
						logger?.LogError(ex, "Error running PostSave event handler:{class}", item.GetType());
					}
				}
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess) {
			var result = base.SaveChanges(acceptAllChangesOnSuccess);
			if (UseChangeEventHandler) {
				ExecutePostSaveEventHandlers().Wait();
			}
			return result;
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
			var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			if(UseChangeEventHandler) {
				await ExecutePostSaveEventHandlers();
			}
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