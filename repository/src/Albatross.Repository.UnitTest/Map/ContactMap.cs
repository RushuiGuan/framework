﻿using Albatross.Repository.UnitTest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.UnitTest.Map {
	public class ContactMap : Albatross.Repository.ByEFCore.EntityMap<Contact> {
		public override void Map(EntityTypeBuilder<Contact> builder) {
			builder.ToTable(nameof(Contact), Constant.Schema);
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
