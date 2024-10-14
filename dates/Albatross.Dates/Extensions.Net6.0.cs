using System;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER
namespace Albatross.Dates {
	public static partial class Extensions {
		/// <summary>
		/// Return the next weekday from a provided date.  If the provided date is Monday, the next 0 weekday is the same date and the 
		/// next 1 weekday would be Tuesday.  If the provided date falls on the weekend, the next 0 weekday and the next 1 weekday are both
		/// Monday.  If the provided date is Friday, the next 0 weekday would be the same day and the next 1 weekday would be Monday.
		/// This method use math to figure out the correct next weekday.  It is more performant compare to the NextBusinessDay implementation where
		/// it loops each day and check if it is a business day.
		/// </summary>
		/// <param name="date">the starting date</param>
		/// <param name="numberOfWeekDays">number of week days to count</param>
		/// <exception cref="ArgumentException">exception will be thrown if the numberOfWeekDays parameter is less than 0</exception>
		public static DateOnly NextWeekday(this DateOnly date, int numberOfWeekDays = 1) {
			if (numberOfWeekDays == 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					return date.AddDays(1);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					return date.AddDays(2);
				} else {
					return date;
				}
			} else if (numberOfWeekDays > 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					date = date.AddDays(-2);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					date = date.AddDays(-1);
				}
				if (numberOfWeekDays >= 5) {
					var weeks = numberOfWeekDays / 5;
					date = date.AddDays(7 * weeks);
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
		/// <summary>
		/// Return the previous weekday from a provided date.  If the provided date is Monday, the previous 0 weekday is the same date and the 
		/// previous 1 weekday would be Friday.  If the provided date falls on the weekend, the previous 0 weekday and the previous 1 weekday are both
		/// Friday.  
		/// </summary>
		/// <param name="date"></param>
		/// <param name="numberOfWeekDays"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">exception will be thrown if the numberOfWeekDays parameter is less than 0</exception>
		public static DateOnly PreviousWeekday(this DateOnly date, int numberOfWeekDays = 1) {
			if (numberOfWeekDays == 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					return date.AddDays(-2);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					return date.AddDays(-1);
				} else {
					return date;
				}
			} else if (numberOfWeekDays > 0) {
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					date = date.AddDays(1);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					date = date.AddDays(2);
				}
				if (numberOfWeekDays >= 5) {
					var weeks = numberOfWeekDays / 5;
					date = date.AddDays(-7 * weeks);
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

		public static bool IsWeekDay(this DateOnly date) => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

		/// <summary>
		/// Similar to NextWeekDay method but the caller can pass in a predicate to check if a date is a business day.  The method is also async, because
		/// holiday data could comes from external resource.  This method is not performant since it has to check every day to see if it is a business day.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="numberOfBusinessDays"></param>
		/// <param name="isBusinessDay"></param>
		/// <returns></returns>
		public static async Task<DateOnly> NextBusinessDay(this DateOnly date, int numberOfBusinessDays = 1, Func<DateOnly, Task<bool>>? isBusinessDay = null) {
			if (isBusinessDay == null) { isBusinessDay = args => Task.FromResult(IsWeekDay(args)); }
			for (int i = 0; i < numberOfBusinessDays; i++) {
				date = date.AddDays(1);
				while (!await isBusinessDay(date)) {
					date = date.AddDays(1);
				}
			}
			while (!await isBusinessDay(date)) {
				date = date.AddDays(1);
			}
			return date;
		}
		public static async Task<DateOnly> PreviousBusinessDay(this DateOnly date, int numberOfBusinessDays = 1, Func<DateOnly, Task<bool>>? isBusinessDay = null) {
			if (isBusinessDay == null) { isBusinessDay = args => Task.FromResult(IsWeekDay(args)); }
			for (int i = 0; i < numberOfBusinessDays; i++) {
				date = date.AddDays(-1);
				while (!await isBusinessDay(date)) {
					date = date.AddDays(-1);
				}
			}
			while (!await isBusinessDay(date)) {
				date = date.AddDays(-1);
			}
			return date;
		}
		/// <summary>
		/// find the nth day of week from the given date
		/// </summary>
		public static DateOnly GetNthDayOfWeek(this DateOnly date, int n, DayOfWeek dayOfWeek) {
			var diff = dayOfWeek - date.DayOfWeek;
			if (diff < 0) {
				diff = 7 - System.Math.Abs(diff);
			}
			return date.AddDays(diff + (n - 1) * 7);
		}

		/// <summary>
		/// Return the difference in total month number between two dates.  The difference between 2022-01-31 and 2022-02-01 is 1
		/// even through they are only 1 day apart
		/// </summary>
		/// <param name="date1"></param>
		/// <param name="date2"></param>
		/// <returns></returns>
		public static int GetMonthDiff(this DateOnly date1, DateOnly date2)
			=> ((date2.Year - date1.Year) * 12) + date2.Month - date1.Month;

		public static int GetNumberOfWeekdays(this DateOnly d1, DateOnly d2) {
			DateOnly startDate, endDate;
			if (d1 < d2) {
				startDate = d1;
				endDate = d2;
			} else {
				startDate = d2;
				endDate = d1;
			}
			int totalDays = endDate.DayNumber - startDate.DayNumber + 1;
			int completeWeeks = totalDays / 7;
			int weekdays = completeWeeks * 5;

			// Calculate the remaining days
			int remainingDays = totalDays % 7;

			// Iterate over the remaining days, checking if each one is a weekday
			for (int i = 0; i < remainingDays; i++) {
				DayOfWeek day = startDate.AddDays(i).DayOfWeek;
				if (day != DayOfWeek.Saturday && day != DayOfWeek.Sunday)
					weekdays++;
			}
			return weekdays;
		}

		public static string ISO8601String(this DateOnly value) => value.ToString(Formatting.ISO8601DateOnly);
		public static string ISO8601String(this TimeOnly value) => value.ToString(Formatting.ISO8601TimeOnly);
		public static DateOnly StartOfMonth(this DateOnly date) => new DateOnly(date.Year, date.Month, 1);
		public static DateOnly EndOfMonth(this DateOnly date) => new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
	}
}
#endif