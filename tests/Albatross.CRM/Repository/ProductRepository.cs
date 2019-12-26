using Albatross.CRM.Model;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Repository {
	public interface IProductRepository : IRepository<Product>{ }

	public class ProductRepository : Repository<Product>, IProductRepository {
		public ProductRepository(IDbSession session) : base(session) { }
	}
}
