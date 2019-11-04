using Albatross.Host.NUnit;
using Albatross.Repository.SqlServer;
using Albatross.Repository.UnitTest.Repository;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestSqlDbSession : TestBase<TestUnitOfWork> {
        public override void RegisterPackages(IServiceCollection services)
        {
			services.UseSqlServer<CRMDbSession>(()=>"server=xyz");
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
