using msg = Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Albatross.Repository.Core;
using System.ComponentModel.DataAnnotations;
using Albatross.CRM.Repository;
using System.Net.Security;

namespace Albatross.CRM.Model {
	public class Customer : MutableEntity<User> {
		public Customer() { }
		public Customer(msg.Customer dto, User user, IProductRepository products) : base(user, products.DbSession.DbContext) {
			Update(dto, user, products, null);
		}

		public int CustomerID {get;private set;}
		[Required]
		public string Name {get;private set;}
		[Required]
		public string Company {get;private set;}

		public virtual ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
		public virtual ICollection<License> Licenses { get; private set; } = new List<License>();
		public ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new List<PurchaseOrder>();

		public void Update(msg.Customer src, User user, IProductRepository products,  DbContext context) {
			Name = src.Name;
			Company = src.Company;
			Licenses.Merge<msg.License, License, string>(src.Licenses,
				args => args.Key,
				args => args.Key,
				(src, dst) => dst.Update(src, user, products, context),
				src => Licenses.Add(new License(src, user, context)),
				dst => Licenses.Remove(dst)); ;
			base.CreateOrUpdate(user, context);
		}
	}
}
