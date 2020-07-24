﻿using Albatross.Repository.Core;
using msg = Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Albatross.CRM.Model {
	public class Contact : MutableEntity<User> {
		private Contact() { }
		public Contact(msg.Contact dto, User user, DbContext context) : base(user, context) {
			Update(dto, user, context);
		}

		public void Update(msg.Contact dto, User user, DbContext context) {
			Name = dto.Name;
			Tag = dto.Tag;
			Addresses.Merge(dto.Addresses,
				args => args.AddressID,
				args => args.AddressID,
				(src, dst) => dst.Update(src, user, context),
				src => Addresses.Add(new Address(src, user, context)),
				dst => Addresses.Remove(dst));

			base.CreateOrUpdate(user, context);
		}


		public int ContactID { get; private set; }
		public string Name { get; private set; }
		public string Tag { get; private set; }
		public string Test { get; private set; }
		public int CustomerID { get; private set; }

		public virtual ICollection<Address> Addresses { get; private set; } = new List<Address>();
		public virtual Customer Customer { get; private set; }
	}
}
