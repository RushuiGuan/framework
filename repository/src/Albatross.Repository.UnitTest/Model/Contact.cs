using Albatross.Repository.Core;
using Albatross.Repository.UnitTest.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest.Model {
	public class Contact : BaseEntity{
		private Contact() { }
		public Contact(ContactDto dto, int user) :base(user){
			Update(dto, user);
		}

		public void Update(ContactDto dto, int user) {
			Name = dto.Name;
			Tag = dto.Tag;
			Addresses.Merge(dto.Addresses, 
				args=>args.AddressID, 
				args=>args.AddressID, 
				(dst, src)=>dst.Update(src, user), 
				src=>Addresses.Add(new Address(src, user)), 
				dst=>Addresses.Remove(dst));

			base.Update(user);
		}


		public int ContactID { get; set; }
		public string Name { get; set; }
        public string Tag { get; set; }
		public string Test { get; set; }

		public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
	}
}
