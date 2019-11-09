using Albatross.Host.Test;
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

namespace Albatross.Repository.UnitTest {
	public class DatabaseTestHost : TestHost {

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCRM(configuration);
		}

		public override void Init(IConfiguration configuration) {
			CRMSetting setting = new GetCRMSettings(configuration).Get();
			Console.WriteLine($"Migrating via {setting.DatabaseProvider}");
			DbContext context;

			if (setting.DatabaseProvider == sqlserver.DatabaseProvider.Name) {
				context = new CRMDbSqlMigrationSession(setting.ConnectionString);
			} else if (setting.DatabaseProvider == postgres.DatabaseProvider.Name) {
				context = new CRMDbPostgresMigrationSession(setting.ConnectionString);
			} else if (setting.DatabaseProvider == sqlite.DatabaseProvider.Name) {
				context = new CRMDbSqlLiteMigrationSession();
			} else {
				throw new UnsupportedDatabaseProviderException(setting.DatabaseProvider);
			}
			context.Database.EnsureDeleted();
			context.Database.Migrate();
		}
	}

	[CollectionDefinition(DatabaseTestHostCollection.Name)]
	public class DatabaseTestHostCollection : ICollectionFixture<DatabaseTestHost> {
		public const string Name = "primary-test-db";
	}
}
