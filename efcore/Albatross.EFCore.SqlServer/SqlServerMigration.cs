using Albatross.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.EFCore.SqlServer {
	public class SqlServerMigration<T> where T : DbSession {
		private readonly ISqlBatchExecution batchExecution;
		private readonly T session;
		private readonly ILogger<SqlServerMigration<T>> logger;

		public SqlServerMigration(ISqlBatchExecution batchExecution, T session, ILogger<SqlServerMigration<T>> logger) {
			this.batchExecution = batchExecution;
			this.session = session;
			this.logger = logger;
		}

		public async Task MigrateEfCore() {
			this.logger.LogInformation("Migrating via {ConnectionString}", session.DbConnection.ConnectionString);
			await session.Database.MigrateAsync();
		}
		public async Task ExecuteDeploymentScript(string location = "Scripts") {
			this.logger.LogInformation("Deploying script via {ConnectionString}", session.DbConnection.ConnectionString);
			var directoryInfo = typeof(T).Assembly.GetAssemblyDirectoryLocation(location);
			if (directoryInfo.Exists) {
				var files = directoryInfo.GetFiles("*.sql").OrderBy(args => args.Name);
				foreach (var file in files) {
					logger.LogInformation("Executing deployment script: {name}", file.Name);
					using (var reader = new StreamReader(file.FullName)) {
						await batchExecution.Execute(session.DbConnection, reader);
					}
				}
			}
		}
		public async Task<bool> HasPendingMigration() {
			var pendingMigrations = await session.Database.GetPendingMigrationsAsync();
			return pendingMigrations.Any();
		}
	}
}