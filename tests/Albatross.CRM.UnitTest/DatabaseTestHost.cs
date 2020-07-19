using Albatross.Hosting.Test;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using Albatross.CRM;
using Albatross.CRM.UnitTest.DbSessions;
using Albatross.Repository.Core;
using sqlite = Albatross.Repository.Sqlite;
using sqlserver = Albatross.Repository.SqlServer;
using postgres = Albatross.Repository.PostgreSQL;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
	public class DatabaseTestHost : TestHost {

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCRM(configuration);
		}

		public override Task InitAsync(IConfiguration configuration) {
			CRMSetting setting = new GetCRMSettings(configuration).Get();
			Console.WriteLine($"Migrating via {setting.DatabaseProvider}");

			using DbContext context = GetDbContext(setting);
			context.Database.EnsureDeleted();
			context.Database.Migrate();
			return base.InitAsync(configuration);
		}

		DbContext GetDbContext(CRMSetting setting){
			if (setting.DatabaseProvider == sqlserver.DatabaseProvider.Name) {
				return new CRMDbSqlMigrationSession(setting.ConnectionString);
			} else if (setting.DatabaseProvider == postgres.DatabaseProvider.Name) {
				return new CRMDbPostgresMigrationSession(setting.ConnectionString);
			} else if (setting.DatabaseProvider == sqlite.DatabaseProvider.Name) {
				return new CRMDbSqlLiteMigrationSession();
			} else {
				throw new UnsupportedDatabaseProviderException(setting.DatabaseProvider);
			}
		}
	}

	[CollectionDefinition(DatabaseTestHostCollection.Name)]
	public class DatabaseTestHostCollection : ICollectionFixture<DatabaseTestHost> {
		public const string Name = "primary-test-db";
	}
}
