using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository {
	public class ImmutableEntity {
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; } = string.Empty;

		protected void Audit(string user) {
			CreatedBy = user;
			CreatedUtc = DateTime.UtcNow;
		}
	}
}