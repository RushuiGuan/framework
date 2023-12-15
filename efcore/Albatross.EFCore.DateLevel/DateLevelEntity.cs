#if NET8_0_OR_GREATER
using System;

namespace Albatross.EFCore.DateLevel {
	public abstract class DateLevelEntity  {
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; } = MaxEndDate;
		public readonly static DateOnly MaxEndDate = DateOnly.MaxValue;

		public abstract bool HasSameValue(DateLevelEntity src);

		public DateLevelEntity(DateOnly startDate) {
			StartDate = startDate;
		}
	}

	public abstract class DateLevelEntity<K> : DateLevelEntity where K : IEquatable<K> {
		public abstract K Key { get; }
		public DateLevelEntity(DateOnly startDate) : base(startDate) { }
	}
}
#endif