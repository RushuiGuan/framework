using Albatross.CRM.Repository;
using Albatross.Mapping.ByAutoMapper;
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
			var result = services.TryUsePostgreSQL<CRMDbSession>(setting, true)||
				services.TryUseSqlServer<CRMDbSession>(setting, false)||
				services.TryUseSqliteInMemory<CRMDbSession>(setting);

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			Albatross.Mapping.Core.Extension.AddMapping(services, typeof(ServiceExtension).Assembly);
			services.AddSingleton<IConfigMapping, ConfigMapping>();
			return services;
		}
	}
}
