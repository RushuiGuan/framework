using msg = Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Albatross.Repository.Core;

namespace Albatross.CRM.Model {
	public class Customer : BaseEntity<User> {
		public Customer() { }
		public Customer(msg.Customer dto, User user) : base(user) {
			Update(dto, user, null);
		}

		public int CustomerID {get;private set;}
		public string Name {get;private set;}
		public string Company {get;private set;}

		public virtual ICollection<Contact> Contacts {get;private set;}
		public virtual ICollection<License> Licenses {get;private set;}

		public void Update(msg.Customer dto, User user, DbContext context) {
			CustomerID = dto.CustomerID;
			Name = dto.Name;
			Company = dto.Company;
			//Licenses.Merge();
			base.Update(user, context);
		}

		public void Input(msg.Customer dto) {
			CustomerID = dto.CustomerID;
			Name = dto.Name;
			Company = dto.Company;
			//Licenses = from item in dto.Licenses select new License()
		}

		public void Update(Customer src) {
			Name = src.Name;
			Company = src.Company;
			Licenses.Merge<License, License, string>(src.Licenses,
				args => args.Key,
				args => args.Key,
				null,
				src => Licenses.Add(src),
				dst => Licenses.Remove(dst));
		}
	}
}
