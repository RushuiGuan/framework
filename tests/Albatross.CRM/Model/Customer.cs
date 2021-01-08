using msg = Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Albatross.Repository.Core;
using System.ComponentModel.DataAnnotations;
using Albatross.CRM.Repository;
using System.Net.Security;
using System.Linq;

namespace Albatross.CRM.Model {
	public class Customer : MutableEntity {
		public Customer() { }
		public Customer(msg.Customer dto, string user, IProductRepository products) {
			Update(dto, user, products, products.DbSession);
		}

		public int CustomerID { get; private set; }
		[Required]
		public string Name { get; private set; }
		[Required]
		public string Company { get; private set; }

		public virtual ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
		public virtual ICollection<License> Licenses { get; private set; } = new List<License>();
		public ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new List<PurchaseOrder>();

		public void Update(msg.Customer src, string user, IProductRepository products, IDbSession session) {
			Name = src.Name;
			Company = src.Company;
			Licenses.Merge<msg.License, License, string>(src.Licenses,
				args => args.Key,
				args => args.Key,
				(src, dst) => dst.Update(src, user, products, session),
				src => Licenses.Add(new License(src, user, session)),
				dst => Licenses.Remove(dst)); ;
			base.CreateOrUpdate(user, session);
		}

		public msg.Customer CreateDto() {
			return new msg.Customer {
				Company = Company,
				Contacts = Contacts?.Select(args => args.CreateDto()).ToArray() ?? new msg.Contact[0],
				CreatedBy = CreatedBy,
				CreatedByUTC = CreatedUTC,
				CustomerID = CustomerID,
				Licenses = Licenses?.Select(args => args.CreateDto()).ToArray() ?? new msg.License[0],
				ModifiedBy = ModifiedBy,
				ModifiedByUTC = ModifiedUTC,
				Name = Name,
			};
		}
	}
}