using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity {
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; }

		protected ImmutableEntity(string createdBy) : this(createdBy, DateTime.UtcNow){
		}
		protected ImmutableEntity(string createdBy, DateTime createdUtc) {
			this.CreatedBy = createdBy;
			this.CreatedUtc = createdUtc;
		}
	}
}
