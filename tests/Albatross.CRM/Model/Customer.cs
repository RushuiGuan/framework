using msg = Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Albatross.Repository.Core;

namespace Albatross.CRM.Model {
	public class Customer : BaseEntity {
		public Customer() { }
		public Customer(msg.Customer dto, int user) : base(user) {
			Update(dto, user);
		}

		public int CustomerID { get; set; }
		public string Name { get; set; }
		public string Company { get; set; }

		public virtual ICollection<Contact> Contacts { get; set; }
		public virtual ICollection<License> Licenses { get; set; }

		public void Update(msg.Customer dto, int user) {
			CustomerID = dto.CustomerID;
			Name = dto.Name;
			Company = dto.Company;
			//Licenses.Merge();
			base.Update(user);
		}

		public void Input(msg.Customer dto) {
			CustomerID = dto.CustomerID;
			Name = dto.Name;
			Company = dto.Company;
			//Licenses = from item in dto.Licenses select new License()
		}

		public void Update(Customer src) {
			Name = src.Name;
			Company = src.Company;
			Licenses.Merge<License, License, string>(src.Licenses,
				args => args.Key,
				args => args.Key,
				null,
				src => Licenses.Add(src),
				dst => Licenses.Remove(dst));
		}
	}

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
