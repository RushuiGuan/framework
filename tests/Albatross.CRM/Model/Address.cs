using Microsoft.EntityFrameworkCore;
using Albatross.Repository.Core;

namespace Albatross.CRM.Model {
	public class Address : MutableEntity {
		protected Address() { }
		public Address(Messages.Address dto, string user, IDbSession session) {
			this.Update(dto, user, session);
		}

		public void Update(Messages.Address dto, string user, IDbSession session) {
			City = dto.City;
			State = dto.State;
			Street = dto.Street;
			base.CreateOrUpdate(user, session);
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