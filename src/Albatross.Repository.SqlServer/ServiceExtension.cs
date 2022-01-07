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
			builder.UseLazyLoadingProxies(false);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseSqlServer(connectionString);
		}

		public static DbContextOptions<T> BuildMigrationOption<T>(string historyTableSchema, string connectionString = DbSession.Any) where T : DbContext {
			DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>();
			builder.UseSqlServer(connectionString, opt => {
				opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
				opt.MigrationsHistoryTable(DbSession.EFMigrationHistory, historyTableSchema); 
			});
			return builder.Options;
		}

		public static IServiceCollection UseSqlServer<T>(this IServiceCollection services, Func<IServiceProvider, string> getConnectionString) where T : DbContext {
			services.AddDbContext<T>((provider, builder) => BuildDefaultOption(builder, getConnectionString(provider)));
			services.AddSingleton<ISqlBatchExecution, SqlBatchExecution>();
			return services;
		}

		/// <summary>
		/// when using context pool, the dbcontext class has to have a constructor with only dbcontextoption parameter 
		/// To create a readonly connection, use the readonly connection string if available (require sql server availability group).  
		/// As an alternative, access the database using an user with only readonly permission
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="services"></param>
		/// <param name="getConnectionString"></param>
		/// <returns></returns>
		public static IServiceCollection UseSqlServerWithContextPool<T>(this IServiceCollection services, Func<IServiceProvider, string>getConnectionString) where T : DbContext {
			services.AddDbContextPool<T>((provider, builder) => BuildDefaultOption(builder, getConnectionString(provider)));
			services.AddSingleton<ISqlBatchExecution, SqlBatchExecution>();
			return services;
		}

		public static bool TryUseSqlServer<T>(this IServiceCollection services, string provider, Func<IServiceProvider, string> getConnectionString, bool useContextPool = true) where T : DbContext {
			if (provider == DatabaseProvider.Name) {
				if (useContextPool) {
					services.AddDbContextPool<T>((provider, builder) => BuildDefaultOption(builder, getConnectionString(provider)));
				} else {
					services.AddDbContext<T>((provider, builder) => BuildDefaultOption(builder, getConnectionString(provider)));
				}
				return true;
			} else {
				return false;
			}
		}
	}
}
