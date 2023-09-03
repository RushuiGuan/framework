using Albatross.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace Albatross.EFCore {
	public abstract class DbSession : DbContext, IDbSession {
		#region constants
		public const string Any = "any";
		public const string EFMigrationHistory = "__EFMigrationsHistory";
		#endregion

		public DbContext DbContext => this;
		public IDbConnection DbConnection => Database.GetDbConnection();

		public DbSession(DbContextOptions option) : base(option) { }

		public virtual Assembly[] EntityModelAssemblies => new Assembly[] { GetType().Assembly };
		public virtual string NamespacePrefix => string.Empty;

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var assembly in EntityModelAssemblies) {
				var items = assembly.GetEntityModels(NamespacePrefix.PostfixIfNotNullOrEmpty('.'));
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
}