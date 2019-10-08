using System;
using System.IO;
using Albatross.Host.NUnit;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestSqlLiteDbSession : TestBase<SqliteUnitOfWork> {


		public override void RegisterPackages(IServiceCollection svc) {
			svc.AddTestDatabase();
			svc.UseSqlite<CRMDbSession>();
		}

		[Test]
		public void SqlLiteScriptGeneration() {
			using (TestUnitOfWork unitOfWork = NewUnitOfWork()) {
				var context = unitOfWork.Get<CRMDbSession>();
				string script = context.GetCreateScript();
				Assert.IsNotEmpty(script);
				using (StreamWriter writer = new StreamWriter("generated.sql")) {
					writer.Write(script);
				}
			}
		}

        
	}
}
