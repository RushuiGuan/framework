using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Maps {
	public class CustomerMap : BaseEntityMap<Customer> {
		public override void Map(EntityTypeBuilder<Customer> builder) {
			builder.ToTable(nameof(Customer), CRMConstant.Schema);
			builder.HasKey(args => args.CustomerID);

			var propertyBuilder = builder.Property(args => args.CustomerID);

			//NpgsqlPropertyBuilderExtensions.UseHiLo(propertyBuilder, CRMConstant.Hilo, CRMConstant.Schema);
			SqlServerPropertyBuilderExtensions.UseHiLo(propertyBuilder, CRMConstant.Hilo, CRMConstant.Schema);

			builder.Property(args => args.Name).HasMaxLength(CRMConstant.NameLength).IsRequired();
			builder.HasIndex(args => args.Name).IsUnique();

			builder.Property(args => args.Company).HasMaxLength(CRMConstant.NameLength).IsRequired();

			builder.HasMany(args => args.Contacts).WithOne(args => args.Customer).HasForeignKey(args => args.CustomerID).IsRequired();
			builder.HasMany(args => args.Licenses).WithOne(args => args.Customer).HasForeignKey(args => args.CustomerID).IsRequired();
			base.Map(builder);
		}
	}
}
