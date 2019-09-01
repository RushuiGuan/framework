using Albatross.Host.NUnit;
using Albatross.Repository.Core;
using Albatross.Repository.UnitTest.Model;
using Albatross.Repository.UnitTest.Repository;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
    [TestFixture]
    public class TestComposite : TestBase<TestUnitOfWork> {
        public override void RegisterPackages(IServiceCollection svc) {
            svc.AddTestDatabase().AddTransient<CompositeRepository>();
        }

        [Test]
        public async Task Run() {
            var item = new Composite {
                Name = "rushui",
                App = "guan",
                Value = "test",
            };

            using (var unitOfWork = NewUnitOfWork()) {
                var repo = unitOfWork.Get<CompositeRepository>();
                repo.RemoveRange(repo.Items.ToArray());
                await repo.SaveChangesAsync();
			}

			using (var unitOfWork = NewUnitOfWork()) {
                var repo = unitOfWork.Get<CompositeRepository>();
                repo.Add(item);
                await repo.SaveChangesAsync();
			}

			using (var unitOfWork = NewUnitOfWork()) {
                var repo = unitOfWork.Get<CompositeRepository>();
                var result = repo.GetItem(item.App, item.Name);
                Assert.NotNull(result);
                Assert.AreEqual(result.Value, item.Value);
            }
        }
    }
}
