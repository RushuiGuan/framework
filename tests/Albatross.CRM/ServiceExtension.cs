using Albatross.CRM.Repository;
using Albatross.Repository.PostgreSQL;
using Albatross.Repository.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CRM {
	public static class ServiceExtension {
		public static IServiceCollection AddCRM(this IServiceCollection services, IConfiguration configuration) {
			var setting = new GetCRMSettings(configuration).Get();
			var result = services.TryUsePostgreSQL<CRMDbSession>(setting.DatabaseProvider, setting.ConnectionString, true) ||
				services.TryUseSqlServer<CRMDbSession>(setting.DatabaseProvider, setting.ConnectionString, false);

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			return services;
		}
	}
}

