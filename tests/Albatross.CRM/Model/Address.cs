using Albatross.Repository.ByEFCore;
using Albatross.CRM.Messages;
using Microsoft.EntityFrameworkCore;
using Albatross.Repository.Core;

namespace Albatross.CRM.Model {
	public class Address : BaseEntity<User> {
		protected Address() { }
		public Address(Messages.Address dto, User user) : base(user) {
			this.Update(dto, user, null);
		}

		public void Update(Messages.Address dto, User user, DbContext context) {
			City = dto.City;
			State = dto.State;
			Street = dto.Street;
			base.Update(user, context);
		}

		public int AddressID {get;private set;}
		public string Type {get;private set;}
		public string Street {get;private set;}
		public string City {get;private set;}
		public string State {get;private set;}


		public int ContactID {get;private set;}
		public virtual Contact Contact {get;private set;}
	}
}