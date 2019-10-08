using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Repository.PostgreSQL {
	public static class ServiceExtension{
		public static void BuildDefaultOption(DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseLazyLoadingProxies(false);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseNpgsql(connectionString);
		}


		public static IServiceCollection UsePostgreSQL<T>(this IServiceCollection services, string connectionString) where T:DbContext {
			services.AddDbContext<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}

		public static IServiceCollection UsePostgreSQLWithContextPool<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}
	}
}
