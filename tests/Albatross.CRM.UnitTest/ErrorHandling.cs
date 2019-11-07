using Albatross.Repository.Core;
using Xunit;
using System;
using System.Threading.Tasks;
using Albatross.CRM.Repository;
using Albatross.CRM.Dto;
using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.UnitTest {
	[Collection(DatabaseTestHostCollection.Name)]
	public class ErrorHandling {
		private readonly DatabaseTestHost host;

		public ErrorHandling(DatabaseTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestUniqueKeyViolationException() {
			Func<Task> func =async () => {
				using (var scope = host.Create()) {
					string name = "test-error-handling-unique-key-violation";
					var repo = scope.Get<ICustomerRepository>();
					repo.Add(new Customer(new CustomerDto { Name = name }, 1));
					repo.Add(new Customer(new CustomerDto { Name = name }, 1));
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
					var customer = new Customer(new CustomerDto { Name = name, }, 1);
					repo.Add(customer);
					await repo.SaveChangesAsync();
				}
			};
			Assert.ThrowsAsync<DbUpdateException>(func);
		}
	}
}
