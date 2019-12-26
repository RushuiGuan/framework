namespace Albatross.CRM.Messages {
	public class Address : BaseEntityDto {

		public int AddressID {get;private set;}
		public string Type {get;private set;}
		public string Street {get;private set;}
		public string City {get;private set;}
		public string State {get;private set;}

		public int ContactID {get;private set;}
	}
}