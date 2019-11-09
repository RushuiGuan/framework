using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Repository.SqlServer {
	public static class ServiceExtension {
		public static void BuildDefaultOption(this DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseLazyLoadingProxies(true);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseSqlServer(connectionString);
		}

		public static DbContextOptions<T> BuildMigrationOption<T>(string historyTableSchema, string connectionString = DbSession.Any) where T : DbContext {
			DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>();
			builder.UseSqlServer(connectionString, opt => opt.MigrationsHistoryTable(DbSession.EFMigrationHistory, historyTableSchema));
			return builder.Options;
		}

		public static IServiceCollection UseSqlServer<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContext<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}

		/// <summary>
		/// when using context pool, the dbcontext class has to have a constructor with only dbcontextoption parameter 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="services"></param>
		/// <param name="getConnectionString"></param>
		/// <returns></returns>
		public static IServiceCollection UseSqlServerWithContextPool<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}

		public static IServiceCollection TryUseSqlServer<T>(this IServiceCollection services, DatabaseConnectionSetting setting, bool useContextPool = true, bool throwIfNotMatched = false) where T : DbContext {
			if (setting.DatabaseProvider == DatabaseProvider.Name) {
				if (useContextPool) {
					services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, setting.ConnectionString));
				} else {
					services.AddDbContext<T>(builder => BuildDefaultOption(builder, setting.ConnectionString));
				}
			} else if (throwIfNotMatched) {
				throw new UnsupportedDatabaseProviderException(setting.DatabaseProvider);
			}
			return services;
		}
	}
}
