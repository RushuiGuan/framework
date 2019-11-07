namespace Albatross.CRM.Dto {
	public class ContactDto : BaseEntityDto {
		public int ContactID { get; set; }
		public string Name { get; set; }
        public string Tag { get; set; }

		public AddressDto[] Addresses { get; set; }
	}
}
