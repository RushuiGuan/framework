using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Dates.Test {
	public class TestDatesExtension {
		[Theory]
		[InlineData(null, "2021-12-06", "2021-12-06")]
		[InlineData("2021-12-07", "2021-12-06", "2021-12-07")]
		public void TestDateOnlyConversion(string? date, string fallback, string expected) {
			DateTime? value = date == null ? null : DateTime.Parse(date);
			var dateOnly = value.ToDateOnly(DateOnly.Parse(fallback));
			Assert.Equal(expected, $"{dateOnly:yyyy-MM-dd}");
		}


		[Theory]
		[InlineData(null, null)]
		[InlineData("2021-12-07", "2021-12-07")]
		public void TestDateOnlyConversion2(string? date, string expected) {
			DateTime? value = date == null ? null : DateTime.Parse(date);
			var dateOnly = value.ToDateOnly();
			Assert.Equal(expected, dateOnly.HasValue ? $"{dateOnly:yyyy-MM-dd}" : null);
		}

		[Theory]
		[InlineData(null, "2021-12-06", "2021-12-06")]
		[InlineData("2021-12-07", "2021-12-06", "2021-12-07")]
		public void TestDateTimeConversion(string? date, string fallback, string expected) {
			DateTime? value = date == null ? null : DateTime.Parse(date);
			var dateOnly = value.ToDateOnly(DateOnly.Parse(fallback));
			Assert.Equal(expected, $"{dateOnly:yyyy-MM-dd}");
		}


		[Theory]
		[InlineData(null, null)]
		[InlineData("2021-12-07", "2021-12-07")]
		public void TestDateTimeConversion2(string? date, string expected) {
			DateTime? value = date == null ? null : DateTime.Parse(date);
			var dateOnly = value.ToDateOnly();
			Assert.Equal(expected, dateOnly.HasValue ? $"{dateOnly:yyyy-MM-dd}" : null);
		}
	}
}
