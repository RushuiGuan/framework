using System;
using System.IO;
using Albatross.Host.NUnit;
using Albatross.Repository.ByEFCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestSqlLiteDbContext : TestBase<TestUnitOfWork> {


		public override void RegisterPackages(IServiceCollection svc) {
			svc.AddTestDatabase();
		}

		[Test]
		public void SqlScriptGeneration() {
			using (TestUnitOfWork unitOfWork = NewUnitOfWork()) {
				var context = unitOfWork.Get<TestingDbContext>();
				string script = context.GetCreateScript();
				Assert.IsNotEmpty(script);
				using (StreamWriter writer = new StreamWriter("generated.sql")) {
					writer.Write(script);
				}
			}
		}
	}
}
