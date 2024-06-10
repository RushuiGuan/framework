using System;
using Xunit;

namespace Albatross.Dates.Test {
	public class TestDatesExtension {
		[Theory]
		[InlineData(null, null)]
		[InlineData("2021-12-07", "2021-12-07")]
		public void TestDateOnlyConversion2(string? date, string? expected) {
			DateTime? value = date == null ? null : DateTime.Parse(date);
			var dateOnly = value.DateOnly();
			Assert.Equal(expected, dateOnly.HasValue ? $"{dateOnly:yyyy-MM-dd}" : null);
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("2021-12-07", "2021-12-07")]
		public void TestDateTimeConversion2(string? date, string? expected) {
			DateTime? value = date == null ? null : DateTime.Parse(date);
			var dateOnly = value.DateOnly();
			Assert.Equal(expected, dateOnly.HasValue ? $"{dateOnly:yyyy-MM-dd}" : null);
		}
	}
}
