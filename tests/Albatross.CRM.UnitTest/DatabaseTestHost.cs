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

namespace Albatross.Repository.UnitTest {
	public class DatabaseTestHost : TestHost{

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCRM(configuration);
		}

		public override void Init(IConfiguration configuration) {
			CRMSetting setting = new GetCRMSettings(configuration).Get();
			Console.WriteLine($"Migrating via {setting.DatabaseProvider}");
			DbContext context;
			if (setting.DatabaseProvider == DbSession.SqlServer) {
				context = new CRMDbSqlMigrationSession(setting.ConnectionString);
			} else if (setting.DatabaseProvider == DbSession.PostgreSQL) {
				context = new CRMDbPostgresMigrationSession(setting.ConnectionString);
			}else if(setting.DatabaseProvider == DbSession.Sqlite) {
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
