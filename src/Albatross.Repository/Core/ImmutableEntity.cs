using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity {
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; protected set; } = String.Empty;

		public ImmutableEntity(string createdBy) {
			this.CreatedBy = createdBy;
			this.CreatedUtc = DateTime.UtcNow;
		}
		/// <summary>
		/// this constructor is used by efcore and unit test
		/// </summary>
		protected internal ImmutableEntity() { }
	}
}