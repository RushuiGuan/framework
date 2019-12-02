namespace Albatross.CRM.Messages {
	public class Customer : BaseEntityDto {
		public int CustomerID { get; set; }
		public string Name { get; set; }
		public string Company { get; set; }

		public Contact[] Contacts { get; set; }
		public License[] Licenses { get; set; }

	}
}
