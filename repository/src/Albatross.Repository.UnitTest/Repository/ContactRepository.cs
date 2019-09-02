using Albatross.Repository.ByEFCore;
using Albatross.Repository.UnitTest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Repository.UnitTest.Repository {
	public class ContactRepository : Repository<Contact>, IContactRepository {
		public ContactRepository(TestingDbContext dbContext) : base(dbContext) {
		}
		public override IQueryable<Contact> Items => base.Items.Include(args=>args.Addresses);

		public Contact Get(string name) {
			return Items.First(args => args.Name == name);
		}
		public bool TryGet(string name, out Contact item) {
			item = Items.FirstOrDefault(args => args.Name == name);
			return item != null;
		}
	}
}
