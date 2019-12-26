using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Maps {
	public class AddressMap : BaseEntityMap<Address> {
		public override void Map(EntityTypeBuilder<Address> builder) {
			builder.HasKey(a => a.AddressID);
			builder.ToTable(nameof(Address), CRMConstant.Schema);

			builder.Property(a => a.Type).HasMaxLength(CRMConstant.NameLength);
			builder.Property(a => a.Street).HasMaxLength(CRMConstant.TitleLength);
			builder.Property(a => a.City).HasMaxLength(CRMConstant.TitleLength);
			builder.Property(a => a.State).HasMaxLength(CRMConstant.NameLength);

			builder.HasOne(a => a.Contact).WithMany(c => c.Addresses).HasForeignKey(a => a.ContactID).IsRequired();
		}
	}
}