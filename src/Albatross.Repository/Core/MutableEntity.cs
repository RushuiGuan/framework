using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class MutableEntity : ImmutableEntity {
		public DateTime ModifiedUtc { get; protected set; }

		[Required]
		[MaxLength(UserNameLength)]
		public string ModifiedBy { get; protected set; }

		protected MutableEntity(string createdBy):base(createdBy) {
			this.ModifiedBy = createdBy;
			this.ModifiedUtc = DateTime.UtcNow;
		}

		public void ValidateAndAudit (string modifiedBy, IDbSession session) {
			this.Validate(session);
			if (session.IsChanged(this)) {
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = modifiedBy;
			}
		}
	}
}
