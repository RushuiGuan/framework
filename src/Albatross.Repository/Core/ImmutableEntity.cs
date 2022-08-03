using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity {
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; } = String.Empty;

		protected void Audit(string user) {
			this.CreatedBy = user;
			this.CreatedUtc = DateTime.UtcNow;
		}
	}
}