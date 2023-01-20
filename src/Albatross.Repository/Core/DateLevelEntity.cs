using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public record struct DateLevelKey {
		public int Id { get; init; }
		public DateTime StartDate { get; init;}
		public DateLevelKey(int id, DateTime start) {
			this.Id= id;
			this.StartDate = start;
		}
	}
	public abstract record class DateLevelEntity {
		public int Id { get; init; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public readonly static DateTime MaxEndDate = new DateTime(9999, 12, 31);
		public DateLevelKey Key => new DateLevelKey(Id, StartDate);

		public DateTime CreatedUtc { get; init; }
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; init; }

		public DateTime ModifiedUtc { get; set; }
		[MaxLength(Constant.UserNameLength)]
		public string ModifiedBy { get; set; }

		public abstract void Update(DateLevelEntity src);
		public abstract bool HasSameValue(DateLevelEntity src);

		// contructor used by efcore
		protected DateLevelEntity(int id, DateTime startDate, DateTime endDate, DateTime createdUtc, string createdBy, DateTime modifiedUtc, string modifiedBy) {
			this.Id = id;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.CreatedBy = createdBy;
			this.CreatedUtc = createdUtc;
			this.ModifiedUtc = modifiedUtc;
			this.ModifiedBy = modifiedBy;
		}

		public DateLevelEntity(int id, DateTime startDate, DateTime? endDate, string createdBy) {
			this.Id = id;
			this.StartDate = startDate;
			this.EndDate = endDate ?? MaxEndDate;
			this.CreatedBy = createdBy;
			this.CreatedUtc = DateTime.UtcNow;
			this.ModifiedBy = createdBy;
			this.ModifiedUtc = DateTime.UtcNow;
		}
	}
}