namespace Albatross.Repository.UnitTest.Dto{
	public class AddressDto : BaseEntityDto{
		
		public int AddressID { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Street { get; set; }

		public int ContactID { get; set; }
	}
}