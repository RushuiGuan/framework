using Albatross.Repository.Core;
using msg = Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Albatross.CRM.Model {
	public class Contact : MutableEntity {
		private Contact() { }
		public Contact(msg.Contact dto, string user, IDbSession session) {
			Update(dto, user, session);
		}

		public void Update(msg.Contact dto, string user, IDbSession session) {
			Name = dto.Name;
			Tag = dto.Tag;
			Addresses.Merge(dto.Addresses,
				args => args.AddressID,
				args => args.AddressID,
				(src, dst) => dst.Update(src, user, session),
				src => Addresses.Add(new Address(src, user, session)),
				dst => Addresses.Remove(dst));

			base.CreateOrUpdate(user, session);
		}


		public int ContactID { get; private set; }
		public string Name { get; private set; }
		public string Tag { get; private set; }
		public string Test { get; private set; }
		public int CustomerID { get; private set; }

		public virtual ICollection<Address> Addresses { get; private set; } = new List<Address>();
		public virtual Customer Customer { get; private set; }

		public msg.Contact CreateDto() {
			return new msg.Contact {
				ContactID = ContactID,
				CreatedBy = CreatedBy,
				Addresses = Addresses?.Select(args => args.CreateDto())?.ToArray() ?? new msg.Address[0],
				CreatedByUTC = CreatedUTC,
				ModifiedBy = ModifiedBy,
				ModifiedByUTC = ModifiedUTC,
				Name = Name,
				Tag = Tag,
			};
		}
	}
}
