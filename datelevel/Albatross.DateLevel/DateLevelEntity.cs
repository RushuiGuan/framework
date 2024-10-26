using System;

namespace Albatross.DateLevel {
	public interface IDateLevelEntity {
		public readonly static DateOnly MaxEndDate = DateOnly.MaxValue;
		DateOnly StartDate { get; }
		DateOnly EndDate { get; }
	}
	public abstract class DateLevelEntity : ICloneable, IDateLevelEntity {
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; } = IDateLevelEntity.MaxEndDate;

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

	public interface IDateLevelEntity<K> : IDateLevelEntity where K : IEquatable<K> {
		public abstract K Key { get; }
	}

	public abstract class DateLevelEntity<K> : DateLevelEntity, IDateLevelEntity<K> where K : IEquatable<K> {
		public abstract K Key { get; }
		public DateLevelEntity(DateOnly startDate) : base(startDate) { }
	}
}