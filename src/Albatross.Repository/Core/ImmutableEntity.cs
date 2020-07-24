using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity {
		public const int UserNameLength = 128;
		public DateTime CreatedUTC { get; protected set; }
		[Required]
		[MaxLength(UserNameLength)]
		public string CreatedBy { get; protected set; }
		protected ImmutableEntity() { }
		protected ImmutableEntity(string user) : this() {
			Create(user);
		}
		public void Create(string user) {
			this.CreatedBy = user;
			this.CreatedUTC = DateTime.UtcNow;
			this.Validate();
		}
		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}
