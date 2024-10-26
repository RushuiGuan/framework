using System;
using System.Collections.Generic;
using Xunit;

namespace Albatross.Text.Test {
	public class TestConversionExtensions {
		public static IEnumerable<object?[]> ConversionData => new List<object?[]> {
			new object[] { "1", 1, typeof(int)},
			new object?[] { null, 0, typeof(int)},

			new object[] { "1", 1L, typeof(long)},
			new object?[] { null, 0L, typeof(long)},

			new object[] { "test", "test", typeof(string)},
			new object[] { "", "", typeof(string)},
			new object?[] { null, null, typeof(string)},

			new object[] { "2024-05-01", new DateTime(2024, 05, 01), typeof(DateTime)},
			new object[] { "", DateTime.MinValue, typeof(DateTime)},
			new object?[] { null, DateTime.MinValue, typeof(DateTime)},

			new object[] { "2024-05-01", new DateOnly(2024, 05, 01), typeof(DateOnly)},
			new object[] { "", DateOnly.MinValue, typeof(DateOnly)},
			new object?[] { null, DateOnly.MinValue, typeof(DateOnly)},

			new object[] { "100", 100M, typeof(decimal)},
			new object[] { "", 0M, typeof(decimal)},
			new object?[] { null, 0M, typeof(decimal)},

			new object[] { "100", 100.0, typeof(double)},
			new object[] { "", 0.0, typeof(double)},
			new object ?[] { null, 0.0, typeof(double) },

			new object?[] { "", null, typeof(int?)},
			new object?[] { null, null, typeof(int?)},
			new object?[] { 1, 1, typeof(int?)},
		};

		[Theory]
		[MemberData(nameof(ConversionData))]
		public void TestValueConversion(string? text, object? expected, Type type) {
			var actual = text.Convert(type);
			Assert.Equal(expected, actual);
		}


		[Fact]
		public void TestEnumParsing() {
			var value = Enum.Parse(typeof(DayOfWeek), "Monday");
			Assert.Equal(DayOfWeek.Monday, value);
			value = Enum.Parse(typeof(DayOfWeek), "monday", true);
			Assert.Equal(DayOfWeek.Monday, value);


			var obj = "monday".Convert(typeof(DayOfWeek));
			Assert.Equal(DayOfWeek.Monday, obj);
		}
	}
}