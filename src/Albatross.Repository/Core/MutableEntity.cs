using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class MutableEntity {
		public const int UserNameLength = 128;
		public DateTime CreatedUTC { get; protected set; }
		public DateTime ModifiedUTC { get; protected set; }

		[Required]
		[MaxLength(UserNameLength)]
		public string CreatedBy { get; protected set; }
		[Required]
		[MaxLength(UserNameLength)]
		public string ModifiedBy { get; protected set; }

		protected MutableEntity() { }
		protected MutableEntity(string user, IDbSession session) {
			CreatedUTC = DateTime.UtcNow;
			CreatedBy = user;
			this.CreateOrUpdate(user, session);
		}
		public void CreateOrUpdate(string user, IDbSession session) {
			if (session.IsChanged(this)) {
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
