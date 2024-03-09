using Albatross.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace Albatross.EFCore {
	public abstract class DbSession : DbContext, IDbSession {
		#region constants
		public const string Any = "any";
		public const string EFMigrationHistory = "__EFMigrationsHistory";
		protected readonly ILogger? logger;
		#endregion

		public DbContext DbContext => this;
		public IDbConnection DbConnection => Database.GetDbConnection();
		public virtual Assembly[] EntityModelAssemblies => new Assembly[] { GetType().Assembly };
		public virtual string NamespacePrefix => string.Empty;

		public DbSession(DbContextOptions option) : base(option) {		}

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