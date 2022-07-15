using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Dates
{
	public static class Extension
	{
		public static DateTime PreviousWeekday(this DateTime date) {
			date = date.Date;
			if (date.DayOfWeek == DayOfWeek.Monday) {
				return date.AddDays(-3);
			} else if (date.DayOfWeek == DayOfWeek.Sunday) {
				return date.AddDays(-2);
			} else {
				return date.AddDays(-1);
			}
		}

		public static DateTime NextWeekday(this DateTime date) {
			date = date.Date;
			if (date.DayOfWeek == DayOfWeek.Friday) {
				return date.AddDays(3);
			} else if (date.DayOfWeek == DayOfWeek.Saturday) {
				return date.AddDays(2);
			} else {
				return date.AddDays(1);
			}
		}
	}
}
