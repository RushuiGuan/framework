using System;
#if NET6_0_OR_GREATER
namespace Albatross.Dates {
	public static class QuarterExtensions {
		/// <summary>
		/// Return the first date of the quarter
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateOnly FirstDateOfQuarter(this DateOnly date) {
			var quarter = date.Quarter();
			return new DateOnly(date.Year, (quarter - 1) * 3 + 1, 1);
		}
		public static DateOnly FirstMonthOfQuarter(this DateOnly date) => FirstDateOfQuarter(date);

		/// <summary>
		/// Return the last date of the quarter
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateOnly LastDateOfQuarter(this DateOnly date) {
			var quarter = date.Quarter();
			return new DateOnly(date.Year, quarter * 3, DateTime.DaysInMonth(date.Year, quarter * 3));
		}

		public static DateOnly LastMonthOfQuarter(this DateOnly date) {
			var quarter = date.Quarter();
			return new DateOnly(date.Year, quarter * 3, 1);
		}

		/// <summary>
		/// Return the quarter of the year
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static int Quarter(this DateOnly date) => (date.Month - 1) / 3 + 1;
		public static string QuarterText(this DateOnly date) => $"{date.Year} Q{date.Quarter()}";
		public static bool IsLastMonthOfQuarter(this DateOnly date) => date.Month % 3 == 0;
	}
}
#endif