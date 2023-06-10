using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Repository.PostgreSQL {
	public static class ServiceExtension {
		public static void BuildDefaultOption(DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseLazyLoadingProxies(false);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseNpgsql(connectionString);
		}

		public static DbContextOptions<T> BuildMigrationOption<T>(string historyTableSchema, string connectionString = DbSession.Any) where T : DbContext {
			DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>();
			builder.UseNpgsql(connectionString, opt => {
				opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
				opt.MigrationsHistoryTable(DbSession.EFMigrationHistory, historyTableSchema); 
			});
			return builder.Options;
		}

		public static IServiceCollection UsePostgreSQL<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContext<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}

		public static IServiceCollection UsePostgreSQLWithContextPool<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}

		public static bool TryUsePostgreSQL<T>(this IServiceCollection services, string provider, string connectionString, bool useContextPool = true) where T : DbContext {
			if (provider == DatabaseProvider.Name) {
				if (useContextPool) {
					services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, connectionString));
				} else {
					services.AddDbContext<T>(builder => BuildDefaultOption(builder, connectionString));
				}
				return true;
			} else {
				return false;
			}
		}
	}
}
