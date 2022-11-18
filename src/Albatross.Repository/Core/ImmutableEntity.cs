using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public record class ImmutableEntity {
		public int Id { get; init; }
		public DateTime CreatedUtc { get; init; }

		[Required]
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; init; }

		public ImmutableEntity(int id, DateTime createdUtc, string createdBy) {
			Id = id;
			CreatedUtc = createdUtc;
			CreatedBy = createdBy;
		}

		public ImmutableEntity(string createdBy) {
			CreatedUtc = DateTime.UtcNow;
			CreatedBy = createdBy;
		}
	}
}