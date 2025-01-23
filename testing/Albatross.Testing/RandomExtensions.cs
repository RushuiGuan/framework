using System;
using System.Collections.Generic;

namespace Albatross.Testing {
	public static class RandomExtensions {
		public static T Random<T>(this IList<T> items) {
			return items[System.Random.Shared.Next(0, items.Count)];
		}

		public static DateOnly RandomDates(DateOnly min, DateOnly max) {
			int days = System.Random.Shared.Next(min.DayNumber, max.DayNumber);
			return DateOnly.FromDayNumber(days);
		}
	}
}