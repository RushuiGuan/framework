using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public record class MutableEntity {
		public DateTime ModifiedUtc { get; protected set; }
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string ModifiedBy { get; protected set; } = string.Empty;
		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; } = string.Empty;

		public void Audit(string user, IDbSession session) {
			if (session.IsNew(this)) {
				CreatedUtc = DateTime.Now;
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = user;
				CreatedBy = user;
			} else if (session.IsChanged(this)) {
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = user;
			}
		}
	}
}