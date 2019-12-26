using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Maps {
	public class ProductMap : BaseEntityMap<Product> {
		public override void Map(EntityTypeBuilder<Product> builder) {
			builder.ToTable(nameof(Product), CRMConstant.Schema);
			builder.HasKey(args => args.ProductID);
			builder.HasAlternateKey(args => args.Name);

			builder.Property(args => args.Name).HasMaxLength(CRMConstant.NameLength).IsRequired();
			builder.Property(args => args.Description).HasMaxLength(CRMConstant.DescriptionLength).IsRequired();
			builder.Property(args => args.AvailableDate);
		}
	}
}
