using Albatross.Repository.Sqlite;
using Albatross.Repository.PostgreSQL;
using Albatross.Repository.SqlServer;
using Albatross.Repository.UnitTest.Repository;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Repository.ByEFCore;
using Albatross.Mapping.Core;
using Albatross.Config.Core;

namespace Albatross.Repository.UnitTest {
	public static class ServiceExtension {
		public static IServiceCollection AddTestDatabase(this IServiceCollection services, TestSetting setting) {
			if (setting.DatabaseProvider == DbSession.SqlServer) {
				services.UseSqlServer<CRMDbSession>(() => setting.ConnectionString);
			} else if (setting.DatabaseProvider == DbSession.PostgreSQL) {
				services.UsePostgreSQL<CRMDbSession>(() => setting.ConnectionString);
			} else if (setting.DatabaseProvider == DbSession.SqlLite) {
				services.UseSqlite<CRMDbSession>();
			} else {
				throw new ConfigurationException("Invalid database provider");
			}
			services.AddScoped<ContactRepository>();
			services.AddScoped<IContactRepository>(provider => provider.GetRequiredService<ContactRepository>());
			Albatross.Mapping.Core.Extension.AddMapping(services, typeof(ServiceExtension).Assembly);
			services.AddSingleton<IConfigMapping, ConfigMapping>();
			return services;
		}
	}
}