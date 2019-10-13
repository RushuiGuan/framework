using Albatross.Host.NUnit;
using Albatross.Repository.Core;
using Albatross.Repository.Sqlite;
using Albatross.Repository.UnitTest.Model;
using Albatross.Repository.UnitTest.Repository;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
    public class TestComposite : TestBase<SqliteUnitOfWork> {
        public override void RegisterPackages(IServiceCollection svc) {
            svc.AddTestDatabase().AddTransient<CompositeRepository>();
			svc.UseSqlite<CRMDbSession>();
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

				repo.Add(item);
				await repo.SaveChangesAsync();

				var result = repo.GetItem(item.App, item.Name);
				Assert.NotNull(result);
				Assert.AreEqual(result.Value, item.Value);
			}
        }
    }
}
