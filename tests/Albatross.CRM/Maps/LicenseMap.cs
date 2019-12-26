using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Maps {
	public class LicenseMap : BaseEntityMap<License> {
		public override void Map(EntityTypeBuilder<License> builder) {
			builder.ToTable(nameof(License), CRMConstant.Schema);
			builder.HasKey(args => args.LicenseID);
			builder.HasAlternateKey(args => args.Key);

			builder.Property(args => args.Key).IsRequired(true);

			builder.HasOne(args => args.Customer).WithMany(args=>args.Licenses).HasForeignKey(args => args.CustomerID).IsRequired(true);
			builder.HasOne(args => args.Product).WithMany().HasForeignKey(args => args.ProductID).IsRequired(true);
			base.Map(builder);
		}
	}
}
