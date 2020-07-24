using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class MutableEntity<UserType> {
		public DateTime CreatedUTC { get; protected set; }
		public DateTime ModifiedUTC { get; protected set; }

		public UserType CreatedBy { get; protected set; }
		public UserType ModifiedBy { get; protected set; }

		protected MutableEntity() { }
		protected MutableEntity(UserType user, DbContext context) {
			CreatedUTC = DateTime.UtcNow;
			CreatedBy = user;
			this.CreateOrUpdate(user, context);
		}
		public void CreateOrUpdate(UserType user, DbContext context) {
			if (this.IsChanged(context)) {
				ModifiedUTC = DateTime.UtcNow;
				ModifiedBy = user;
			}
			this.Validate();
		}

		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}
