using Albatross.Host.NUnit;
using Albatross.Repository.Sqlite;
using Albatross.Repository.UnitTest.Dto;
using Albatross.Repository.UnitTest.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class TestFirstBehavior : TestBase<TestScope> {
		private void CreateAddress(ContactDto c) {
			const int AddressCount = 10;
			c.Addresses = new AddressDto[AddressCount];
			for (int i = 0; i < AddressCount; i++) {
				c.Addresses[i] = new AddressDto {
					City = $"c{i}",
					State = c.Name,
					Street = $"s{i}",
				};
			}
		}

		[Test]
		public async Task Run() {
			string name = "test";
			using (var unitOfWork = NewUnitOfWork()) {
				var contacts = unitOfWork.Get<ContactRepository>();
				ContactDto dto = new ContactDto {
					 Name = name,
				};
				CreateAddress(dto);

				contacts.Add(new Model.Contact(dto, 0));
				await contacts.DbSession.SaveChangesAsync();

				var result = contacts.Items.Where(args => args.Name == name).First();
				Assert.NotNull(result.Addresses);
				Assert.Greater(result.Addresses.Count, 1);
				 
				result = await contacts.Items.Where(args => args.Name == name).FirstAsync();
				Assert.Greater(result.Addresses.Count, 1);
			}
		}
	}
}
