using Albatross.Repository.ByEFCore;
using Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.CRM.Model {
	public class Address : BaseEntity {
		protected Address() { }
		public Address(Messages.Address dto, int user) : base(user) {
			this.Update(dto, user);
		}

		public void Update(Messages.Address dto, int user) {
			City = dto.City;
			State = dto.State;
			Street = dto.Street;
			base.Update(user);
		}

		public int AddressID { get; set; }
		public string Type { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }


		public int ContactID { get; set; }
		public virtual Contact Contact { get; set; }
	}

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