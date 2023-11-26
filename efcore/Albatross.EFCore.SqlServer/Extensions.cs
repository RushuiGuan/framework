using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Albatross.EFCore.SqlServer {
	public static class Extensions {
		public static void DefaultDbContextOptionBuilder(SqlServerDbContextOptionsBuilder builder) {
			builder.CommandTimeout(100);
		}
		public static void BuildDefaultOption(this DbContextOptionsBuilder builder, string connectionString, Action<SqlServerDbContextOptionsBuilder>? dbcontextOptionBuilder = null) {
			builder.EnableDetailedErrors(true);
			builder.EnableSensitiveDataLogging(true);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseSqlServer(connectionString, dbcontextOptionBuilder ?? DefaultDbContextOptionBuilder);
		}

		public static DbContextOptions<T> BuildMigrationOption<T>(string historyTableSchema, string connectionString = DbSession.Any) where T : DbContext {
			DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>();
			builder.EnableDetailedErrors(true);
			// builder.UseLazyLoadingProxies(false);
			builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			builder.UseSqlServer(connectionString, opt => {
				opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
				opt.MigrationsHistoryTable(DbSession.EFMigrationHistory, historyTableSchema);
			});
			return builder.Options;
		}

		public static IServiceCollection AddSqlServer<T>(this IServiceCollection services, Func<IServiceProvider, string> getConnectionString, Action<SqlServerDbContextOptionsBuilder>? dbcontextOptionBuilder = null) where T : DbContext {
			services.AddDbContext<T>((provider, builder) => BuildDefaultOption(builder, getConnectionString(provider), dbcontextOptionBuilder));
			services.TryAddSingleton<ISqlBatchExecution, SqlBatchExecution>();
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
		public static IServiceCollection AddSqlServerWithContextPool<T>(this IServiceCollection services, Func<IServiceProvider, string> getConnectionString, Action<SqlServerDbContextOptionsBuilder>? dbcontextOptionBuilder = null) where T : DbContext {
			services.AddDbContextPool<T>((provider, builder) => BuildDefaultOption(builder, getConnectionString(provider), dbcontextOptionBuilder ?? DefaultDbContextOptionBuilder));
			services.TryAddSingleton<ISqlBatchExecution, SqlBatchExecution>();
			return services;
		}
	}
}