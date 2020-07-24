using Albatross.Repository.Core;
using Xunit;
using System;
using System.Threading.Tasks;
using Albatross.CRM.Repository;
using Albatross.CRM.Messages;
using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.UnitTest {
	[Collection(DatabaseTestHostCollection.Name)]
	public class ErrorHandling {
		private readonly DatabaseTestHost host;

		public ErrorHandling(DatabaseTestHost host) {
			this.host = host;
		}

		string Admin = "admin";

		[Fact]
		public void TestUniqueKeyViolationException() {
			Func<Task> func =async () => {
				using (var scope = host.Create()) {
					string name = "test-error-handling-unique-key-violation";
					var repo = scope.Get<ICustomerRepository>();
					var products = scope.Get<IProductRepository>();
					repo.Add(new CRM.Model.Customer(new CRM.Messages.Customer { Name = name }, Admin, products));
					repo.Add(new CRM.Model.Customer(new CRM.Messages.Customer { Name = name }, Admin, products));
					await repo.SaveChangesAsync();
				}
			};
			Assert.ThrowsAsync<DbUpdateException>(func);
		}

		[Fact]
		public void TestMissingRequiredFieldException() {
			Func<Task> func = async () => {
				using (var scope = host.Create()) {
					string name = "test-error-handling-required-field-violation";
					var repo = scope.Get<ICustomerRepository>();
					var products = scope.Get<IProductRepository>();
					var customer = new CRM.Model.Customer(new CRM.Messages.Customer { Name = name, }, Admin, products);
					repo.Add(customer);
					await repo.SaveChangesAsync();
				}
			};
			Assert.ThrowsAsync<DbUpdateException>(func);
		}
	}
}
