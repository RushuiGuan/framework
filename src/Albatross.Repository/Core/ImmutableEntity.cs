using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public record class ImmutableEntity {
		public DateTime CreatedUtc { get; private set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; private set; } = string.Empty;

		public void Audit(string user) {
			this.CreatedBy = user;
			this.CreatedUtc = DateTime.UtcNow;
		}
	}
}