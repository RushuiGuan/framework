using System;

namespace Albatross.EFCore {
	public abstract class DateLevelEntity  {
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; } = MaxEndDate;
		public readonly static DateTime MaxEndDate = new DateTime(9999, 12, 31);

		public abstract bool HasSameValue(DateLevelEntity src);

		public DateLevelEntity(DateTime startDate) {
			StartDate = startDate;
		}
	}

	public abstract class DateLevelEntity<K> : DateLevelEntity where K : IEquatable<K> {
		public abstract K Key { get; }
		public DateLevelEntity(DateTime startDate) : base(startDate) { }
	}
}