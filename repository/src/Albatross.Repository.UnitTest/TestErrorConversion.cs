using System.Linq;
using Albatross.Repository.UnitTest.Repository;
using Autofac;
using NUnit.Framework;
using Albatross.Repository.Core;
using Albatross.Host.NUnit;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Repository.UnitTest.Dto;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestErrorConversion : TestBase<TestUnitOfWork> {
		public override void RegisterPackages(IServiceCollection svc) {
			svc.AddTestDatabase().AddTransient<ContactRepository>();
		}

		static readonly string Tag = typeof(TestErrorConversion).FullName;

		public override void OneTimeSetUp(TestUnitOfWork unitOfWork) {
			base.OneTimeSetUp(unitOfWork);
			var repo = unitOfWork.Get<ContactRepository>();
			repo.RemoveRange(repo.Items.Where(args => args.Tag == Tag));
			repo.SaveChangesAsync().Wait();
		}


		[Test]
		public void TestUniqeKeyViolationException() {
			TestDelegate testDelegate = new TestDelegate(async () => {
				string name = nameof(TestUniqeKeyViolationException);
				using (var unitOfWork = NewUnitOfWork()) {
					var repo = unitOfWork.Get<ContactRepository>();
					repo.Add(new Model.Contact(new ContactDto { Name = name }, 1));
					repo.Add(new Model.Contact(new ContactDto { Name = name }, 1));
					await repo.SaveChangesAsync();
				}
			});
			Assert.Catch<UniqueKeyViolationException>(testDelegate);
		}

		[Test]
		public void TestMissingRequiredFieldException() {
			TestDelegate testDelegate = new TestDelegate(async () => {
				using (var unitOfWork = NewUnitOfWork()) {
					var repo = unitOfWork.Get<ContactRepository>();
					repo.Add(new Model.Contact(new ContactDto { Name = null }, 1));
					await repo.SaveChangesAsync();
				}
			});
			Assert.Catch<MissingRequiredFieldException>(testDelegate);
		}
	}
}
