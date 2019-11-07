namespace Albatross.CRM.Dto {
	public class CustomerDto : BaseEntityDto {
		public int CustomerID { get; set; }
		public string Name { get; set; }
		public string Company { get; set; }

		public ContactDto[] Contacts { get; set; }
	}
}
