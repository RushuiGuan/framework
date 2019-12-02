namespace Albatross.CRM.Messages {
	public class Address : BaseEntityDto {

		public int AddressID { get; set; }
		public string Type { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }

		public int ContactID { get; set; }
	}
}