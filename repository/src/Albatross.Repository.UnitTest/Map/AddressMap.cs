using Albatross.Repository.ByEFCore;
using Albatross.Repository.UnitTest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest.Map {
	public class AddressMap : EntityMap<Address> {
		public override void Map(EntityTypeBuilder<Address> builder) {
			builder.HasKey(a => a.AddressID);
			builder.ToTable(nameof(Address), Constant.Schema);

			builder.Property(a => a.Street).HasMaxLength(100);
			builder.Property(a => a.City).HasMaxLength(100);
			builder.Property(a => a.State).HasMaxLength(50);

			builder.Property(a => a.Created);
			builder.Property(a => a.CreatedBy);

			builder.Ignore(a => a.Modified);
			builder.Ignore(a => a.ModifiedBy);
			//builder.Property(a => a.Modified);
			//builder.Property(a => a.ModifiedBy);

			builder.HasOne(a => a.Contact).WithMany(c => c.Addresses).HasForeignKey(a => a.ContactID).IsRequired();
		}
	}
}
