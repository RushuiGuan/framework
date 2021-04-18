using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Maps {
	public class ContactMap : Albatross.Repository.ByEFCore.EntityMap<Contact> {
		public override void Map(EntityTypeBuilder<Contact> builder) {
			builder.ToTable(nameof(Contact), CRMConstant.Schema);
			builder.HasKey(args => args.ContactID);
			builder.HasAlternateKey(c => c.Name);
			builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
			builder.Property(c => c.Tag);

			builder.Property(a => a.CreatedUtc);
			builder.Property(a => a.CreatedBy);
			builder.Property(a => a.ModifiedUtc);
			builder.Property(a => a.ModifiedBy);
			builder.Ignore(a => a.Test);

			builder.HasMany(c => c.Addresses).WithOne(a => a.Contact).HasForeignKey(a => a.ContactID).IsRequired();
		}
	}
}
