namespace Albatross.CRM.Messages {
	public class Contact : BaseEntity {
		public int ContactID {get; set;}
		public string Name {get; set;}
        public string Tag {get; set;}

		public Address[] Addresses {get; set;}
	}
}
