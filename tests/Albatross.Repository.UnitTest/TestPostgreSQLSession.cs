using Albatross.Host.NUnit;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Repository.PostgreSQL;
using NUnit.Framework;
using System;
using Albatross.Repository.UnitTest.Repository;
using Albatross.Repository.ByEFCore;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestPostgreSQLSession : TestBase<TestScope> {
        public override void RegisterPackages(IServiceCollection services)
        {
			services.UsePostgreSQL<CRMDbSession>(()=> DbSession.Any);
            base.RegisterPackages(services);
        }

        [Test]
        public void SqlScriptGeneration() {
            using (TestScope unitOfWork = NewUnitOfWork()) {
                var context = unitOfWork.Get<CRMDbSession>();
                string script = context.GetCreateScript();
                Console.WriteLine(script);
            }
        }
	}
}
