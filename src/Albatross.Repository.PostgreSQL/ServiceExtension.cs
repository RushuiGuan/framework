using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Repository.PostgreSQL {
	public static class ServiceExtension{
		public static void BuildDefaultOption(DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseLazyLoadingProxies(false);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseNpgsql(connectionString);
		}

		public static void BuildMigrationOption(this DbContextOptionsBuilder builder, string historyTableSchema, string connectionString = "any") {
			builder.UseNpgsql(connectionString, opt => opt.MigrationsHistoryTable("__EFMigrationsHistory", historyTableSchema));
		}

		public static IServiceCollection UsePostgreSQL<T>(this IServiceCollection services, Func<string> getConnectionString) where T:DbContext {
			services.AddDbContext<T>(builder => BuildDefaultOption(builder, getConnectionString()));
			return services;
		}

		public static IServiceCollection UsePostgreSQLWithContextPool<T>(this IServiceCollection services, Func<string> getConnectionString) where T : DbContext {
			services.AddDbContextPool<T>(builder => BuildDefaultOption(builder, getConnectionString()));
			return services;
		}
	}
}
