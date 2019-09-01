using System.Linq;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.UnitTest.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest.Repository {
	public class CompositeRepository : Repository<Composite>{
		public CompositeRepository(TestDbContext dbContext) : base(dbContext) {
		}
	}
}
