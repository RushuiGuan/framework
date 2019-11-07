using Albatross.CRM.Repository;
using Albatross.Mapping.Core;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Albatross.Repository.Dto;
using Albatross.Repository.PostgreSQL;
using Albatross.Repository.Sqlite;
using Albatross.Repository.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CRM {
	public static class ServiceExtension {
		public static IServiceCollection AddCRM(this IServiceCollection services, IConfiguration configuration) {
			var setting = new GetCRMSettings(configuration).Get();
			if (setting.DatabaseProvider == DbSession.SqlServer) {
				services.UseSqlServer<CRMDbSession>(() => setting.ConnectionString);
			} else if (setting.DatabaseProvider == DbSession.PostgreSQL) {
				services.UsePostgreSQL<CRMDbSession>(() => setting.ConnectionString);
			} else if (setting.DatabaseProvider == DbSession.Sqlite) {
				services.UseSqlite<CRMDbSession>();
			} else {
				throw new UnsupportedDatabaseProviderException(setting.DatabaseProvider);
			}

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			Albatross.Mapping.Core.Extension.AddMapping(services, typeof(ServiceExtension).Assembly);
			services.AddSingleton<IConfigMapping, ConfigMapping>();
			return services;
		}
	}
}
