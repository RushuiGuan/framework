namespace Albatross.CRM.Messages {
	public class Contact : BaseEntityDto {
		public int ContactID { get; set; }
		public string Name { get; set; }
        public string Tag { get; set; }

		public Address[] Addresses { get; set; }
	}
}
