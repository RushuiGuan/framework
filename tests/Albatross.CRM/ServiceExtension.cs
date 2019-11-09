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
			services.TryUsePostgreSQL<CRMDbSession>(setting, false)
				.TryUseSqlServer<CRMDbSession>(setting, false)
				.TryUseSqlite<CRMDbSession>(setting, true);

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			Albatross.Mapping.Core.Extension.AddMapping(services, typeof(ServiceExtension).Assembly);
			services.AddSingleton<IConfigMapping, ConfigMapping>();
			return services;
		}
	}
}
