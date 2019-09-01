using Albatross.Host.NUnit;
using Albatross.Repository.UnitTest.Dto;
using Albatross.Repository.UnitTest.Model;
using Albatross.Repository.UnitTest.Repository;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TransactionTest : TestBase<TestUnitOfWork> {
		public override void RegisterPackages(IServiceCollection svc) {
			svc.AddTestDatabase();
		}
		[Test]
		public async Task Run() {
			using (var unitOfWork = NewUnitOfWork()) {
				using (var t = unitOfWork.Get<TestDbContext>().BeginTransaction()) {
					var contacts = unitOfWork.Get<IContactRepository>();
					contacts.Add(new Contact(new ContactDto {
						Name = nameof(TransactionTest),
					}, 1));
					await contacts.DbSession.SaveChangesAsync();
				}
			}
		}
	}
}
