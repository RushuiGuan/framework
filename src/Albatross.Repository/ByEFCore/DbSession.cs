using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Repository.ByEFCore {
	public abstract class DbSession : DbContext, IDbSession {
		#region constants
		public const string Any = "any";
		public const string EFMigrationHistory = "__EFMigrationsHistory";
		#endregion

		public DbContext DbContext => this;
		public IDbConnection DbConnection => this.Database.GetDbConnection();

		public DbSession(DbContextOptions option) : base(option) { }

		public virtual Assembly[] EntityModelAssemblies => new Assembly[] { this.GetType().Assembly };

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var assembly in EntityModelAssemblies) {
				var items = assembly.GetEntityModels();
				foreach (var item in items) {
					item.Build(modelBuilder);
				}
			}
		}

		public bool IsNew(object t) {
			var entry = DbContext.Entry(t);
			return entry.State == EntityState.Added || entry.State == EntityState.Detached;
		}

		public bool IsChanged(object t) {
			var entry = DbContext.Entry(t);
			return entry.State != EntityState.Unchanged;
		}
	}
	public abstract class ReadOnlyDbSession : DbSession {
		protected ReadOnlyDbSession(DbContextOptions option) : base(option) {
		}

		public override int SaveChanges() {
			throw new InvalidOperationException("This db context is readonly");
		}
		public override int SaveChanges(bool acceptAllChangesOnSuccess) {
			throw new InvalidOperationException("This db context is readonly");
		}
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
			throw new InvalidOperationException("This db context is readonly");
		}
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
			throw new InvalidOperationException("This db context is readonly");
		}
	}
}