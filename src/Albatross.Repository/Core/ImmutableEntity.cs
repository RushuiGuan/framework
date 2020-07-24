using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity<UserType> {
		public DateTime CreatedUTC { get; protected set; }
		public UserType CreatedBy { get; protected set; }
		protected ImmutableEntity() { }
		protected ImmutableEntity(UserType user) : this() {
			Create(user);
		}
		public void Create(UserType user) {
			this.CreatedBy = user;
			this.CreatedUTC = DateTime.UtcNow;
			this.Validate();
		}
		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}
