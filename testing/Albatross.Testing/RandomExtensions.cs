using System;

namespace Albatross.Testing {
	public static class RandomExtensions {
		public static T Random<T>(this T[] items) {
			return items[System.Random.Shared.Next(0, items.Length)];
		}

		public static DateOnly RandomDates(DateOnly min, DateOnly max) {
			int days = System.Random.Shared.Next(min.DayNumber, max.DayNumber);
			return DateOnly.FromDayNumber(days);
		}
	}
}