using Albatross.EFCore.SqlServer;
using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using Sample.EFCore.Models;

namespace Sample.EFCore {
	public static class Extensions {
		public static IServiceCollection AddSample(this IServiceCollection services) {
			services.AddConfig<SampleConfig>();
			services.AddScoped<ISampleDbSession>(provider => provider.GetRequiredService<SampleDbSession>());
			services.AddSqlServerWithContextPool<SampleDbSession>(provider => provider.GetRequiredService<SampleConfig>().ConnectionString);
			return services;
		}
	}
}
