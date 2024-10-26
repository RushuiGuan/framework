using System;
using System.Linq;

namespace Albatross.DateLevel {
	public record class DateLevelValue<T> : IDateLevelEntity {
		public DateLevelValue(T value, DateOnly startDate, DateOnly endDate) {
			Value = value;
			StartDate = startDate;
			EndDate = endDate;
		}

		public T Value { get; }
		public DateOnly StartDate { get; }
		public DateOnly EndDate { get; }
	}

	public record class DateLevelValue<T, K> : IDateLevelEntity where K : IEquatable<K> {
		public DateLevelValue(T value, K key, DateOnly startDate, DateOnly endDate) {
			Value = value;
			this.Key = key;
			StartDate = startDate;
			EndDate = endDate;
		}

		public T Value { get; }
		public K Key { get; }
		public DateOnly StartDate { get; }
		public DateOnly EndDate { get; }
	}
}