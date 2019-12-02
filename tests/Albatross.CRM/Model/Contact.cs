using Albatross.Repository.Core;
using Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Model {
	public class Contact : BaseEntity{
		private Contact() { }
		public Contact(Messages.Contact dto, int user) :base(user){
			Update(dto, user);
		}

		public void Update(Messages.Contact dto, int user) {
			Name = dto.Name;
			Tag = dto.Tag;
			Addresses.Merge(dto.Addresses, 
				args=>args.AddressID, 
				args=>args.AddressID, 
				(src, dst) =>dst.Update(src, user), 
				src=>Addresses.Add(new Address(src, user)), 
				dst=>Addresses.Remove(dst));

			base.Update(user);
		}


		public int ContactID { get; set; }
		public string Name { get; set; }
        public string Tag { get; set; }
		public string Test { get; set; }
		public int CustomerID { get; set; }

		public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
		public virtual Customer Customer { get; set; }
	}

	public class ContactMap : Albatross.Repository.ByEFCore.EntityMap<Contact> {
		public override void Map(EntityTypeBuilder<Contact> builder) {
			builder.ToTable(nameof(Contact), CRMConstant.Schema);
			builder.HasKey(args => args.ContactID);
			builder.HasAlternateKey(c => c.Name);
			builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
			builder.Property(c => c.Tag);

			builder.Property(a => a.Created);
			builder.Property(a => a.CreatedBy);
			builder.Property(a => a.Modified);
			builder.Property(a => a.ModifiedBy);
			builder.Ignore(a => a.Test);

			builder.HasMany(c => c.Addresses).WithOne(a => a.Contact).HasForeignKey(a => a.ContactID).IsRequired();
		}
	}
}
