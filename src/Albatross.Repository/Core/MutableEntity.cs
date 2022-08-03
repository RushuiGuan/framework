using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class MutableEntity  {
		public DateTime ModifiedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string ModifiedBy { get; protected set; }

		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; }


		protected MutableEntity(string createdBy, DateTime createdUtc, string modifiedBy, DateTime modifiedUtc) {
			this.CreatedBy = createdBy;
			this.CreatedUtc = createdUtc;
			this.ModifiedBy = modifiedBy;
			this.ModifiedUtc = modifiedUtc;
		}
		protected MutableEntity(string createdBy) :this(createdBy, DateTime.UtcNow, createdBy, DateTime.UtcNow){		}

		public void Audit (string modifiedBy, IDbSession session) {
			if (session.IsChanged(this)) {
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = modifiedBy;
			}
		}
	}
}
