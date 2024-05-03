#if NET6_0_OR_GREATER
using System;

namespace Albatross.DateLevel {
	public abstract class DateLevelEntity : ICloneable {
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; } = MaxEndDate;
		public readonly static DateOnly MaxEndDate = DateOnly.MaxValue;

		/// <summary>
		/// Returns true if the object has the same value as another DateLevelEntity object.  This is not the same as an equal comparison
		/// since two objects could have different start and end dates.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public abstract bool HasSameValue(DateLevelEntity src);

		public abstract object Clone();

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