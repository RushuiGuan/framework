using Albatross.Host.NUnit;
using Albatross.Repository.ByEFCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestSqlDbSession : TestBase<TestUnitOfWork> {
        public override void RegisterPackages(IServiceCollection services)
        {
            services.AddTestDatabase();
            base.RegisterPackages(services);
        }

        [Test]
        public void SqlScriptGeneration() {
            using (TestUnitOfWork unitOfWork = NewUnitOfWork()) {
                var context = unitOfWork.Get<SqlServerCreateScriptDbSession>();
                string script = context.GetCreateScript();
                Console.WriteLine(script);
            }
        }
	}
}
