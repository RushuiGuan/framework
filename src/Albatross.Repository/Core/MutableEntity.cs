using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class MutableEntity {
		public DateTime ModifiedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string ModifiedBy { get; protected set; } = String.Empty;

		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; } = String.Empty;

		public MutableEntity(string createdBy) {
			this.CreatedBy = createdBy;
			this.ModifiedBy = createdBy;
			this.CreatedUtc = DateTime.UtcNow;
			this.ModifiedUtc = DateTime.UtcNow;
		}

		/// <summary>
		/// this constructor is used by efcore and unit test
		/// </summary>
		protected internal MutableEntity() { }

		public void Audit(string modifiedBy, IDbSession session) {
			if (session.IsChanged(this)) {
				ModifiedUtc = DateTime.UtcNow;
				ModifiedBy = modifiedBy;
			}
		}
	}
}