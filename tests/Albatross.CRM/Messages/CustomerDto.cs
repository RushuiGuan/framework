namespace Albatross.CRM.Messages {
	public class Customer : BaseEntityDto {
		public int CustomerID {get;private set;}
		public string Name {get;private set;}
		public string Company {get;private set;}

		public Contact[] Contacts {get;private set;}
		public License[] Licenses {get;private set;}

	}
}
