using System;
namespace Albatross.EFCore.DateLevel {
	public abstract class DateTimeLevelEntity  {
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; } = MaxEndDate;
		public readonly static DateTime MaxEndDate = new DateTime(9999, 12, 31);

		public abstract bool HasSameValue(DateTimeLevelEntity src);

		public DateTimeLevelEntity(DateTime startDate) {
			StartDate = startDate;
		}
	}

	public abstract class DateTimeLevelEntity<K> : DateTimeLevelEntity where K : IEquatable<K> {
		public abstract K Key { get; }
		public DateTimeLevelEntity(DateTime startDate) : base(startDate) { }
	}
}
