using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CRM.Repository {

	public interface ICustomerRepository : IRepository<Customer> {
		Customer Get(string name);
		Customer Get(int id);
		bool TryGet(string name, out Customer item);
	}

	public class CustomerRepository : Repository<Customer>, ICustomerRepository {
		public CustomerRepository(CRMDbSession dbContext) : base(dbContext) {
		}

		public Customer Get(int id) {
			return Items.First(args => args.CustomerID == id);
		}

		public Customer Get(string name) {
			return Items.First(args => args.Name == name);
		}
		public bool TryGet(string name, out Customer item) {
			item = Items.FirstOrDefault(args => args.Name == name);
			return item != null;
		}
	}
}