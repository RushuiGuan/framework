#if NET6_0_OR_GREATER
using System;

namespace Albatross.Dates {
	public static class DateOnlyExtensions {
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
	}
}
#endif