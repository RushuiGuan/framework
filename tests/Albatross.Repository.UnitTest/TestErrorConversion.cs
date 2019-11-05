using System.Linq;
using Albatross.Repository.UnitTest.Repository;
using Autofac;
using NUnit.Framework;
using Albatross.Repository.Core;
using Albatross.Host.NUnit;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Repository.UnitTest.Dto;
using System.Threading.Tasks;
using Albatross.Repository.Sqlite;

namespace Albatross.Repository.UnitTest {
	[Ignore("Need to figure out sql error handling for different data providers")]
	[TestFixture]
	public class TestErrorConversion : TestBase<TestScope> {
		static readonly string Tag = typeof(TestErrorConversion).FullName;

		[Test]
		public void TestUniqeKeyViolationException() {
			AsyncTestDelegate testDelegate = new AsyncTestDelegate(async () => {
				string name = nameof(TestUniqeKeyViolationException);
				using (var unitOfWork = NewUnitOfWork()) {
					unitOfWork.Get<CRMDbSession>().Database.EnsureCreated();
					var repo = unitOfWork.Get<ContactRepository>();
					repo.Add(new Model.Contact(new ContactDto { Name = name }, 1));
					repo.Add(new Model.Contact(new ContactDto { Name = name }, 1));
					await repo.SaveChangesAsync();
				}
			});
			Assert.CatchAsync<UniqueKeyViolationException>(testDelegate);
		}

		[Test]
		public void TestMissingRequiredFieldException() {
			AsyncTestDelegate testDelegate = new AsyncTestDelegate(async () => {
				using (var unitOfWork = NewUnitOfWork()) {
					unitOfWork.Get<CRMDbSession>().Database.EnsureCreated();
					var repo = unitOfWork.Get<ContactRepository>();
					repo.Add(new Model.Contact(new ContactDto { Name = null }, 1));
					await repo.SaveChangesAsync();
				}
			});
			Assert.CatchAsync<MissingRequiredFieldException>(testDelegate);
		}
	}
}
