using Albatross.Reflection;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Repository.SqlServer {
	public class SqlServerMigration<T> where T: DbSession {
		private readonly ISqlBatchExecution batchExecution;
		private readonly T session;
		private readonly ILogger<SqlServerMigration<T>> logger;

		public SqlServerMigration(ISqlBatchExecution batchExecution, T session,ILogger<SqlServerMigration<T>> logger) {
			this.batchExecution = batchExecution;
			this.session = session;
			this.logger = logger;
		}

		public async Task Migrate() {
			this.logger.LogInformation("Migrating via {ConnectionString}", session.DbConnection.ConnectionString);
			session.Database.Migrate();
			
			var directoryInfo = typeof(T).Assembly.GetAssemblyLocation("Scripts");
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
	}
}
