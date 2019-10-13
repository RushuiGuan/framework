using Albatross.Host.NUnit;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Repository.PostgreSQL;
using NUnit.Framework;
using System;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestPostgreSQLSession : TestBase<TestUnitOfWork> {
        public override void RegisterPackages(IServiceCollection services)
        {
			services.AddTestDatabase();
			services.UsePostgreSQL<CRMDbSession>(()=>"server=xyz");
            base.RegisterPackages(services);
        }

        [Test]
        public void SqlScriptGeneration() {
            using (TestUnitOfWork unitOfWork = NewUnitOfWork()) {
                var context = unitOfWork.Get<CRMDbSession>();
                string script = context.GetCreateScript();
                Console.WriteLine(script);
            }
        }
	}
}
