using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Repository.Sqlite {
	public static class ServiceExtension {
		public const string ConnectionString = "Data Source=:memory:";
		/// <summary>
		/// Being a in memory database, we have to prevent efcore from manage the database connections, since all data is lost when
		/// the connection is closed.  When declare and assign the connection manually, the connection will be closed\disposed when
		/// the scope is disposed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection UseSqlite<T>(this IServiceCollection services) where T : DbContext {
			services.AddDbContext<T>(builder => {
				SqliteConnection sqlLiteConnection = new SqliteConnection(ConnectionString);
				sqlLiteConnection.Open();
				builder.EnableDetailedErrors(true);
				builder.EnableSensitiveDataLogging(true);
				builder.UseLazyLoadingProxies(true);
				builder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
				builder.UseSqlite(sqlLiteConnection);
			}, ServiceLifetime.Singleton);
			return services;
		}

		public static IServiceCollection TryUseSqlite<T>(this IServiceCollection services, DatabaseConnectionSetting setting, bool useContextPool, bool throwIfNotMatched = false) where T : DbContext {
			if (setting.DatabaseProvider == DatabaseProvider.Name) {
				services.UseSqlite<T>();
			} else if (throwIfNotMatched) {
				throw new UnsupportedDatabaseProviderException(setting.DatabaseProvider);
			}
			return services;
		}

		public static DbContextOptions<T> BuildMigrationOption<T>(string historyTableSchema) where T : DbContext {
			DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>();
			builder.UseSqlite(ConnectionString, opt => opt.MigrationsHistoryTable(DbSession.EFMigrationHistory, historyTableSchema));
			return builder.Options;
		}
	}
}
