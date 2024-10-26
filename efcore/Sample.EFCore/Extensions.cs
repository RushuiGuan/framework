using Albatross.Config;
using Albatross.EFCore;
using Albatross.EFCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Sample.EFCore.Models;

namespace Sample.EFCore {
	public static class Extensions {
		public static IServiceCollection AddSample(this IServiceCollection services) {
			services.AddDbSessionEvents();
			services.AddConfig<SampleConfig>();
			services.AddScoped<ISampleDbSession>(provider => provider.GetRequiredService<SampleDbSession>());
			services.AddScoped<MyDataService>();
			services.AddSqlServerWithContextPool<SampleDbSession>(provider => provider.GetRequiredService<SampleConfig>().ConnectionString);
			return services;
		}
	}
}