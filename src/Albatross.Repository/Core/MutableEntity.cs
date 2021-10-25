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

		public void Update (string modifiedBy, IDbSession session) {
			if (session.IsChanged(this)) {
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = modifiedBy;
			}
			this.Validate(session);
		}
	}
}
