using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.EFCore.PostgreSQL {
	public static class Extensions {
		public static void BuildDefaultOption(DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
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

		public static IServiceCollection AddPostgreSQL<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContext<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}

		public static IServiceCollection AddPostgreSQLWithContextPool<T>(this IServiceCollection services, string connectionString) where T : DbContext {
			services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, connectionString));
			return services;
		}
	}
}