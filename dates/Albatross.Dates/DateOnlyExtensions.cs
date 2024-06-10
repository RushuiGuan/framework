#if NET6_0_OR_GREATER
using System;

namespace Albatross.Dates {
	public static class DateOnlyExtensions {
		public static DateOnly DateOnly(this DateTime dateTime) => System.DateOnly.FromDateTime(dateTime);
		public static DateOnly DateOnly(this DateTimeOffset dateTimeOffset) => System.DateOnly.FromDateTime(dateTimeOffset.Date);
		public static DateTime DateTime(this DateOnly date) => date.ToDateTime(TimeOnly.MinValue);

		public static DateOnly? DateOnly(this DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.DateOnly() : null;
		public static DateOnly? DateOnly(this DateTimeOffset? dateTimeOffset) => dateTimeOffset.HasValue ? dateTimeOffset.Value.DateOnly() : null;
		public static DateTime? DateTime(this DateOnly? date) => date.HasValue ? date.DateTime() : null;
	}
}
#endif