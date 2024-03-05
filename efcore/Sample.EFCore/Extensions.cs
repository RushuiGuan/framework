using Albatross.EFCore.SqlServer;
using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using Sample.EFCore.Models;
using Albatross.EFCore.AutoCacheEviction;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Albatross.EFCore;

namespace Sample.EFCore {
	public static class Extensions {
		public static IServiceCollection AddSample(this IServiceCollection services) {
			services.TryAddSingleton<IDbChangeEventHandlerFactory, DbChangeEventHandlerFactory>();
			services.AddConfig<SampleConfig>();
			services.AddAutoCacheEviction();
			services.AddScoped<ISampleDbSession>(provider => provider.GetRequiredService<SampleDbSession>());
			services.AddScoped<MyDataService>();
			services.AddSqlServerWithContextPool<SampleDbSession>(provider => provider.GetRequiredService<SampleConfig>().ConnectionString);
			return services;
		}
	}
}
