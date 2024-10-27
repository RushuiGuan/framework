using Albatross.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace Albatross.EFCore {
	/// <summary>
	/// To build entity models, override the <see cref="OnModelCreating"/> method. The code generator will genenerate the extension method 
	/// `BuildEntityModels` for class <see cref="ModelBuilder"/>.  The method will register entity builder for all classes that implements 
	/// the <see cref="IBuildEntityModel"/> interface in the same assembly.
	/// ```csharp
	///		protected override void OnModelCreating(ModelBuilder modelBuilder) {
	///			modelBuilder.BuildEntityModels();
	///			modelBuilder.HasDefaultSchema(My.Schema.Sample);
	///		}
	/// ```
	/// The DbSession change the default string type from nvarchar to varchar.  If nvarchar is required, apply Unicode(true) attribute to column or
	/// override the ConfigureConventions method to undo globally.  We prefer varchar because all new databases are created with utf8 collation and nvarchar
	/// is never required.
	/// </summary>
	public abstract class DbSession : DbContext, IDbSession {
		#region constants
		public const string Any = "any";
		public const string EFMigrationHistory = "__EFMigrationsHistory";
		protected readonly ILogger? logger;
		#endregion

		public DbContext DbContext => this;
		public IDbConnection DbConnection => Database.GetDbConnection();
		public virtual string NamespacePrefix => string.Empty;

		public DbSession(DbContextOptions option) : base(option) { }
		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
			base.ConfigureConventions(configurationBuilder);
			configurationBuilder.Properties<string>().AreUnicode(false);
		}
	}
}