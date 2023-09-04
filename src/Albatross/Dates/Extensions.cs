using System;

namespace Albatross.Dates {
	public static class Extensions {
		public static DateTime NextWeekday(this DateTime date, int numberOfWeekDays = 1) {
			if(numberOfWeekDays == 0) {
				if(date.DayOfWeek == DayOfWeek.Sunday) {
					return date.Date.AddDays(1);
				}else if(date.DayOfWeek == DayOfWeek.Saturday) {
					return date.Date.AddDays(2);
				} else {
					return date.Date;
				}
			}else if(numberOfWeekDays > 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					date = date.AddDays(-2);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					date = date.AddDays(-1);
				}
				if (numberOfWeekDays >= 5) {
					var weeks = numberOfWeekDays / 5;
					date = date.Date.AddDays(7 * weeks);
				}
				var remaining = numberOfWeekDays % 5;
				if ((int)date.DayOfWeek + remaining > 5) {
					remaining = remaining + 2;
				}
				date = date.AddDays(remaining);
				return date;
			} else {
				throw new ArgumentException($"{nameof(numberOfWeekDays)} parameter has to be greater or equal to 0");
			}
		}

		public static DateTime PreviousWeekday(this DateTime date, int numberOfWeekDays = 1) {
			if(numberOfWeekDays == 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					return date.Date.AddDays(-2);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					return date.Date.AddDays(-1);
				} else {
					return date.Date;
				}
			}else if (numberOfWeekDays > 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					date = date.AddDays(1);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					date = date.AddDays(2);
				}
				if (numberOfWeekDays >= 5) {
					var weeks = numberOfWeekDays / 5;
					date = date.Date.AddDays(-7 * weeks);
				}
				var remaining = numberOfWeekDays % 5;
				if ((int)date.DayOfWeek - remaining < 1) {
					remaining = remaining + 2;
				}
				date = date.AddDays(-1 * remaining);
				return date;
			} else {
				throw new ArgumentException($"{nameof(numberOfWeekDays)} parameter has to be greater or equal to 0");
			}
		}
	}
}
