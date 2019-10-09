using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Repository.SqlServer {
	public static class ServiceExtension{
		public static void BuildDefaultOption(DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseLazyLoadingProxies(false);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseSqlServer(connectionString);
		}


		public static IServiceCollection UseSqlServer<T>(this IServiceCollection services, Func<string> getConnectionString) where T:DbContext {
			services.AddDbContext<T>(builder => BuildDefaultOption(builder, getConnectionString()));
			return services;
		}

		/// <summary>
		/// when using context pool, the dbcontext class has to have a constructor with only dbcontextoption parameter 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="services"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static IServiceCollection UseSqlServerWithContextPool<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}
	}
}
