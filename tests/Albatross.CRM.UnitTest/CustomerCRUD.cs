using Albatross.Repository.Core;
using Albatross.Mapping.Core;
using System.Threading.Tasks;
using Xunit;
using Albatross.Hosting.Test;
using System;
using Albatross.CRM.Repository;
using Albatross.CRM.Model;
using msg = Albatross.CRM.Messages;

namespace Albatross.Repository.UnitTest {
	[Collection(DatabaseTestHostCollection.Name)]
	public class CustomerCRUD {
		private readonly DatabaseTestHost host;
		public CustomerCRUD(DatabaseTestHost host) {
			this.host = host;
		}

		User Admin { get; } = new User(1, "admin");
		User User { get; } = new User(2, "user");

		const string CustomerName = nameof(CustomerCRUD);

		private Task RemoveCustomer(string name) {
			using (var scope = host.Create()) {
				var customers = scope.Get<ICustomerRepository>();
				if (customers.TryGet(name, out CRM.Model.Customer item)) {
					customers.Remove(item);
				}
				return customers.SaveChangesAsync();
			}
		}
		private msg.Customer GetCustomer(string name, out CRM.Model.Customer model) {
			using (var scope = host.Create()) {
				var customers = scope.Get<ICustomerRepository>();
				model = customers.Get(name);
				return scope.Get<IMapperFactory>().Map<Customer, msg.Customer>(model);
			}
		}
		private async Task<CRM.Messages.Customer> CreateCustomer(TestScope scope, string name, User user) {
			CRM.Model.Customer model;
			var customers = scope.Get<ICustomerRepository>();
			var products = scope.Get<IProductRepository>();
			var dto = new CRM.Messages.Customer { Name = name, Company = name, };
			model = new CRM.Model.Customer(dto, user, products);
			customers.Add(model);
			await customers.SaveChangesAsync();
			return scope.Get<IMapperFactory>().Map<CRM.Model.Customer, CRM.Messages.Customer>(model);
		}

		private async Task<CRM.Messages.Customer> UpdateCustomer(TestScope scope, CRM.Messages.Customer dto, User user) {
			CRM.Model.Customer model;
			var customers = scope.Get<ICustomerRepository>();
			var products = scope.Get<IProductRepository>();
			model = customers.Get(dto.CustomerID);
			model.Update(dto, user, products, customers.DbSession.DbContext);
			await customers.SaveChangesAsync();
			return scope.Get<IMapperFactory>().Map<CRM.Model.Customer, CRM.Messages.Customer>(model);
		}

		[Fact]
		public async Task Create() {
			using (var scope = host.Create()) {
				string name = "test-customer-create";

				var dto = await CreateCustomer(scope, name, Admin);
				var customers = scope.Get<ICustomerRepository>();
				var model = customers.Get(name);
				Assert.Equal(Admin.UserID, model.CreatedBy.UserID);
				Assert.Equal(Admin.UserID, model.ModifiedBy.UserID);

				Assert.Equal(dto.Name, (string)model.Name);
				Assert.True(model.CreatedUTC < DateTime.UtcNow);
				Assert.True(model.ModifiedUTC < DateTime.UtcNow);
			}
		}

		[Fact]
		public async Task Update() {
			using (var scope = host.Create()) {
				string name = "test-customer-update";
				var dto = await CreateCustomer(scope, name, Admin);

				dto.Name = "test-customer-update-123";
				dto = await UpdateCustomer(scope, dto, User);

				var customers = scope.Get<ICustomerRepository>();
				var model = customers.Get(dto.Name);

				Assert.Equal(1, model.CreatedBy.UserID);
				Assert.Equal(2, model.ModifiedBy.UserID);
				Assert.True(model.ModifiedUTC < DateTime.UtcNow);
				Assert.True(model.ModifiedUTC > model.CreatedUTC);
			}
		}

		[Fact]
		public async Task Delete() {
			using (var scope = host.Create()) {
				string name = "test-customer-delete";
				var dto = await CreateCustomer(scope, name, Admin);

				var customers = scope.Get<ICustomerRepository>();
				var model = customers.Get(dto.Name);
				customers.Remove(model);
				await customers.SaveChangesAsync();
				Assert.Throws<InvalidOperationException>(() => model = customers.Get(dto.Name));
			}
		}
	}
}
