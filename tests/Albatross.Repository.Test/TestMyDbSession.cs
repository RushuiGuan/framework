using Albatross.Repository.Core;
using Albatross.Repository.SqlServer;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestMyDbSession : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestMyDbSession(MyTestHost host) {
			this.host = host;
		}
		[Fact]
		public void GenerateScript() {
			using var scope = host.Create();
			var session = scope.Get<MyDbSession>();
			string script = session.GetCreateScript();

			
			using (var file = File.OpenWrite($"{Path.GetDirectoryName(this.GetType().Assembly.Location)}\\MyDbSession.sql")) {
				using (var writer = new StreamWriter(file)) {
					writer.Write(script);
					writer.Flush();
					file.SetLength(file.Position);
				}
			}

			var stream = Assembly.GetExecutingAssembly()
				.GetManifestResourceStream("Albatross.Repository.Test.MyDbSession.sql")
				?? throw new InvalidOperationException("embedded stream doesn't exist");
			var expected = new StreamReader(stream).ReadToEnd();
			Assert.Equal(expected, script);
		}


		[Fact]
		public async Task RunEfMigrate() {
			var scope = host.Create();
			var migration = scope.Get<SqlServerMigration<MySqlServerMigration>>();
			await migration.MigrateEfCore();
		}
	}
}
