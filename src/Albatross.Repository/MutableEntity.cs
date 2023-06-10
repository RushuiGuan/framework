using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository {
	public class MutableEntity {
		public DateTime ModifiedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string ModifiedBy { get; protected set; } = string.Empty;

		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; } = string.Empty;

		public void Audit(string user, IDbSession session) {
			if (session.IsNew(this)) {
				CreatedBy = user;
				CreatedUtc = DateTime.UtcNow;
				ModifiedBy = user;
				ModifiedUtc = DateTime.UtcNow;
			} else if (session.IsChanged(this)) {
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = user;
			}
		}
	}
}