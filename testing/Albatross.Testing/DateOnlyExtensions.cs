using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albatross.Testing {
#if NET6_0_OR_GREATER
	public static class DateOnlyExtensions {
		static Lazy<Random> random = new Lazy<Random>(() => new Random());

		public static DateOnly RandomDates(DateOnly min, DateOnly max) {
			int days = random.Value.Next(min.DayNumber, max.DayNumber);
			return DateOnly.FromDayNumber(days);
		}
	}
#endif
}