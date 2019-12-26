namespace Albatross.CRM.Messages {
	public class Contact : BaseEntityDto {
		public int ContactID {get;private set;}
		public string Name {get;private set;}
        public string Tag {get;private set;}

		public Address[] Addresses {get;private set;}
	}
}
