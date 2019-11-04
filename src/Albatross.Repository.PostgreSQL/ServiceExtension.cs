using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Repository.PostgreSQL {
	public static class ServiceExtension{
		public static void BuildDefaultOption(DbContextOptionsBuilder builder, string connectionString) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseLazyLoadingProxies(true);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseNpgsql(connectionString);
		}

		public static DbContextOptions<T> BuildMigrationOption<T>(string historyTableSchema, string connectionString = DbSession.Any) where T:DbContext{
			DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>();
			builder.UseNpgsql(connectionString, opt => opt.MigrationsHistoryTable(DbSession.EFMigrationHistory, historyTableSchema));
			return builder.Options;
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
