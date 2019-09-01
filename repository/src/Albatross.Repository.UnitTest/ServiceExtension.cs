using Albatross.Repository.UnitTest.Repository;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Repository.ByEFCore;
using Albatross.Mapping.Core;

namespace Albatross.Repository.UnitTest {
    public static class ServiceExtension
    {
		public static IServiceCollection AddTestDatabase(this IServiceCollection services) {
            services.AddSingleton<GetTestDatabaseConnectionString>();
            services.AddScoped<TestDbContext>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddCustomEFCore(typeof(ServiceExtension).Assembly);
			services.AddScoped<ContactRepository>();
			Albatross.Mapping.Core.Extension.AddMapping(services, typeof(ServiceExtension).Assembly);
			services.AddSingleton<IConfigMapping, ConfigMapping>();
			return services;
		}
    }
}
