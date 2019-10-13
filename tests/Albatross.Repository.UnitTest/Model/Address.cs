using Albatross.Repository.UnitTest.Dto;

namespace Albatross.Repository.UnitTest.Model {
	public class Address : BaseEntity {
		protected Address() { }
		public Address(AddressDto dto, int user) :base(user){
			this.Update(dto, user);
		}

		public void Update(AddressDto dto, int user) {
			City = dto.City;
			State = dto.State;
			Street = dto.Street;
			base.Update(user);
		}

		public int AddressID { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Street { get; set; }


		public int ContactID { get; set; }
		public virtual Contact Contact { get; set; }
	}
}