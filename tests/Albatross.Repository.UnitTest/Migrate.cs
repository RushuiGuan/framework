using Albatross.Repository.UnitTest.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest {
	public class Migrate {
		private readonly IConfiguration configuration;

		public Migrate(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public void Run() {
			TestSetting setting = this.configuration.GetSection(TestSetting.Key).Get<TestSetting>();
			Console.WriteLine($"Migrating via {setting.DatabaseProvider}");

			if (setting.DatabaseProvider == "sqlserver") {
				var context = new CRMDbSqlMigrationSession(setting.ConnectionString);
				context.Database.EnsureDeleted();
				context.Database.Migrate();
			} else if (setting.DatabaseProvider == "postgresql") {
				var context = new CRMDbPostgresMigrationSession(setting.ConnectionString);
				context.Database.EnsureDeleted();
				context.Database.Migrate();
			}
		}
	}
}
