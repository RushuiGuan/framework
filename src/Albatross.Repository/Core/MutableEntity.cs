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

		public void CreateOrUpdate(string user, IDbSession session) {
			if (session.IsNew(this)) {
				CreatedBy = user;
				CreatedUTC = DateTime.UtcNow;
				ModifiedBy = user;
				ModifiedUTC = DateTime.UtcNow;
			} else if (session.IsChanged(this)) {
				ModifiedUTC = DateTime.UtcNow;
				ModifiedBy = user;
			}
			this.Validate(session);
		}

		public virtual void Validate(IDbSession session) {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}
