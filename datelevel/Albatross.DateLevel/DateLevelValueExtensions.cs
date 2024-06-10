using System;
using System.Collections.Generic;

namespace Albatross.DateLevel {
	public static class DateLevelValueExtensions {
		public static T Effective<T>(this IEnumerable<DateLevelValue<T>> values, DateOnly date) {
			foreach (var value in values) {
				if (date >= value.StartDate && date <= value.EndDate) {
					return value.Value;
				}
			}
			throw new InvalidOperationException($"No value found effective {date:yyyy-MM-dd}");
		}
	}
}
