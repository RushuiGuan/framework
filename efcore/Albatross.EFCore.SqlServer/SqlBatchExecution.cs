using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.EFCore.SqlServer {
	public interface ISqlBatchExecution {
		Task Execute(IDbConnection dbConnection, System.IO.TextReader reader);
	}
	public class SqlBatchExecution : ISqlBatchExecution {
		Regex goRegex = new Regex(@"^\s*go\s*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private readonly ILogger<SqlBatchExecution> logger;

		public SqlBatchExecution(ILogger<SqlBatchExecution> logger) {
			this.logger = logger;
		}

		public async Task Execute(IDbConnection db, System.IO.TextReader reader) {
			StringBuilder sb = new StringBuilder();
			for (string? line = reader.ReadLine(); line != null; line = reader.ReadLine()) {
				if (goRegex.IsMatch(line)) {
					if (!string.IsNullOrWhiteSpace(sb.ToString())) {
						logger.LogInformation("Executing: {query}", sb);
						await db.ExecuteAsync(sb.ToString());
					}
					sb.Length = 0;
				} else {
					sb.AppendLine(line);
				}
			}
			if (!string.IsNullOrWhiteSpace(sb.ToString())) {
				logger.LogInformation("Executing: {query}", sb);
				await db.ExecuteAsync(sb.ToString());
			}
		}
	}
}