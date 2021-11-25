using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity {
		public const int UserNameLength = 128;
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(UserNameLength)]
		public string CreatedBy { get; protected set; }

		protected ImmutableEntity(string createdBy) {
			this.CreatedBy = createdBy;
			this.CreatedUtc = DateTime.UtcNow;
		}
		
		public virtual void Validate(IDbSession session) {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}
