using Albatross.CRM.UnitTest.DbSessions;
using Albatross.Repository.Core;
using Xunit;

namespace Albatross.Repository.UnitTest {
	public class ScriptGeneration {
        [Fact]
        public void SqlServerScriptGeneration() {
			using (var context = new CRMDbSqlMigrationSession()) {
				string script = context.GetCreateScript();
				Assert.NotEmpty(script);
			}
		}
		[Fact]
		public void PostgreSQLScriptGeneration() {
			using (var context = new CRMDbPostgresMigrationSession()) {
				string script = context.GetCreateScript();
				Assert.NotEmpty(script);
			}
		}
	}
}
