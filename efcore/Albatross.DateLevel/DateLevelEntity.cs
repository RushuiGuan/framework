using System;

namespace Albatross.DateLevel {
	public abstract class DateLevelEntity : ICloneable {
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; } = MaxEndDate;
		public readonly static DateOnly MaxEndDate = DateOnly.MaxValue;

		/// <summary>
		/// Returns true if the object are the same with the exception of the start and end date
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
