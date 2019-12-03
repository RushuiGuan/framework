using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class BaseEntity<UserType> {
		public DateTime Created { get; protected set; }
		public DateTime Modified { get; protected set; }

		public UserType CreatedBy { get; protected set; }
		public UserType ModifiedBy { get; protected set; }

		public DateTimeOffset CreatedUTC => new DateTimeOffset(Created, TimeSpan.Zero);
		public DateTimeOffset ModifiedUTC => new DateTimeOffset(Modified, TimeSpan.Zero);

		protected BaseEntity() {
			Created = DateTime.UtcNow;
			Modified = DateTime.UtcNow;
		}
		protected BaseEntity(UserType user) : this() {
			CreatedBy = user;
			ModifiedBy = user;
		}
		public void Update(UserType user, DbContext context) {
			if (context == null || this.IsChanged(context)) {
				Modified = DateTime.UtcNow;
				ModifiedBy = user;
			}
		}

		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}
