using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Model {
	public class Product :BaseEntity{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime AvailableDate { get; set; }


	}
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
