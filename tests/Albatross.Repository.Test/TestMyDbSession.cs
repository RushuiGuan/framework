using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
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

			using (var file = File.OpenWrite(@"C:\app\framework\tests\Albatross.Repository.Test\MyDbSession.sql")) {
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
	}
}
