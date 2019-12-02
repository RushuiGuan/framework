using Albatross.CRM.Messages;
using Albatross.CRM.Model;
using Albatross.CRM.Repository;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.UnitTest {
	[Collection(DatabaseTestHostCollection.Name)]
	public class TransactionTest {
		private readonly DatabaseTestHost host;

		public TransactionTest(DatabaseTestHost host) {
			this.host = host;
		}

		[Fact]
		public async Task Run() {
			using (var scope = host.Create()) {
				using (var t = scope.Get<CRMDbSession>().BeginTransaction()) {
					string name = "customer-test-transaction";
					var contacts = scope.Get<ICustomerRepository>();
					contacts.Add(new CRM.Model.Customer(new CRM.Messages.Customer { Name = name, Company = name, }, 1));
					await contacts.DbSession.SaveChangesAsync();
				}
			}
		}
	}
}
