using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Model {
	public class License : BaseEntity {
		protected License() { }
		public License(int user) : base(user) {
		}
		public int LicenseID { get; private set; }
		public int ProductID { get; private set; }
		public int CustomerID { get; private set; }

		[StringLength(256)]
		[Required]
		public string Key { get; private set; }

		public virtual Customer Customer { get; private set; }
		public virtual Product Product { get; private set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }

		public void Validate() {
			if(BeginDate > EndDate) {
				throw new ValidationException("Begin date should be before end date");
			}
		}
	}

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
