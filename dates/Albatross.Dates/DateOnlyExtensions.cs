#if NET6_0_OR_GREATER
using System;

namespace Albatross.Dates {
	public static class Dates {
		public static DateOnly ToDateOnly(this DateTime? dateTime, DateOnly fallbackValue) {
			if(dateTime.HasValue) {
				return DateOnly.FromDateTime(dateTime.Value);
			} else {
				return fallbackValue;
			}
		}
		public static DateOnly? ToDateOnly(this DateTime? dateTime){
			if(dateTime.HasValue) {
				return DateOnly.FromDateTime(dateTime.Value);
			} else {
				return null;
			}
		}
		public static DateTime? ToDateTime(this DateOnly? date) {
			if (date.HasValue) {
				return date.Value.ToDateTime(TimeOnly.MinValue);
			} else {
				return null;
			}
		}
		public static DateTime ToDateTime(this DateOnly? date, DateTime fallbackValue) {
			if (date.HasValue) {
				return date.Value.ToDateTime(TimeOnly.MinValue);
			} else {
				return fallbackValue;
			}
		}
		public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
	}
}
#endif