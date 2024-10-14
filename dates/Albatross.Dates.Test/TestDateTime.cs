using System;
using Xunit;
using Albatross.Dates;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Albatross.Dates.Test {
	public class TestDateTime {
		[Fact]
		public void TestDateFormat() {
			var date = new DateTime(2023, 6, 1, 8, 10, 20, DateTimeKind.Local);
			string text = date.ToString("yyyy-MM-ddTHH:mm:ss.fffz");
			Assert.Equal("2023-06-01T08:10:20.000-4", text);


			string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
			text = date.ToString(format);
			Assert.Equal("2023-06-01T08:10:20.000Z", text);

			text = "2023-06-25T21:47:44.060Z";
			date = DateTime.ParseExact(text, format, null);
			Assert.True(date.Kind == DateTimeKind.Local);
		}

		[Theory]
		[InlineData("2022-07-08", 1, "2022-07-11")]
		[InlineData("2022-07-09", 1, "2022-07-11")]
		[InlineData("2022-07-10", 1, "2022-07-11")]
		[InlineData("2022-07-07", 1, "2022-07-08")]

		[InlineData("2023-09-01", 0, "2023-09-01")]
		[InlineData("2023-09-02", 0, "2023-09-04")]
		[InlineData("2023-09-03", 0, "2023-09-04")]
		[InlineData("2023-09-04", 0, "2023-09-04")]

		[InlineData("2023-08-28", 5, "2023-09-04")]
		[InlineData("2023-08-29", 5, "2023-09-05")]
		[InlineData("2023-08-30", 5, "2023-09-06")]
		[InlineData("2023-08-31", 5, "2023-09-07")]
		[InlineData("2023-09-01", 5, "2023-09-08")]
		[InlineData("2023-09-02", 5, "2023-09-08")]
		[InlineData("2023-09-03", 5, "2023-09-08")]

		[InlineData("2023-09-01", 1, "2023-09-04")]
		[InlineData("2023-09-01", 2, "2023-09-05")]
		[InlineData("2023-08-28", 0, "2023-08-28")]
		[InlineData("2023-08-28", 1, "2023-08-29")]
		[InlineData("2023-08-28", 2, "2023-08-30")]
		[InlineData("2023-08-28", 3, "2023-08-31")]
		[InlineData("2023-08-28", 4, "2023-09-01")]
		[InlineData("2023-08-15", 20, "2023-09-12")]
		[InlineData("2023-08-15", 21, "2023-09-13")]
		[InlineData("2023-08-15", 19, "2023-09-11")]


		[InlineData("2023-09-02", 1, "2023-09-04")]
		[InlineData("2023-09-03", 1, "2023-09-04")]
		[InlineData("2023-09-02", 2, "2023-09-05")]
		[InlineData("2023-09-03", 2, "2023-09-05")]
		public void TestNextWeekday(string dateText, int days, string expectedText) {
			var date = DateTime.Parse(dateText);
			var expected = DateTime.Parse(expectedText);
			var actual = date.NextWeekday(days);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TestNextWeekdayException() {
			var date = DateTime.Parse("2023-01-01");
			Assert.Throws<ArgumentException>(() => date.NextWeekday(-1));
		}

		[Theory]
		[InlineData("2022-07-09", 1, "2022-07-08")]
		[InlineData("2022-07-10", 1, "2022-07-08")]
		[InlineData("2022-07-11", 1, "2022-07-08")]
		[InlineData("2022-07-08", 1, "2022-07-07")]

		[InlineData("2023-08-28", 0, "2023-08-28")]
		[InlineData("2023-08-27", 0, "2023-08-25")]
		[InlineData("2023-08-26", 0, "2023-08-25")]
		[InlineData("2023-08-25", 0, "2023-08-25")]
		[InlineData("2023-08-24", 0, "2023-08-24")]

		[InlineData("2023-08-28", 5, "2023-08-21")]
		[InlineData("2023-08-27", 5, "2023-08-21")]
		[InlineData("2023-08-26", 5, "2023-08-21")]
		[InlineData("2023-08-25", 5, "2023-08-18")]
		[InlineData("2023-08-24", 5, "2023-08-17")]


		[InlineData("2023-08-30", 1, "2023-08-29")]
		[InlineData("2023-08-30", 2, "2023-08-28")]
		[InlineData("2023-08-30", 3, "2023-08-25")]
		[InlineData("2023-08-30", 4, "2023-08-24")]
		[InlineData("2023-08-30", 5, "2023-08-23")]

		[InlineData("2023-08-30", 20, "2023-08-02")]
		[InlineData("2023-08-30", 19, "2023-08-03")]
		[InlineData("2023-08-30", 21, "2023-08-01")]
		public void TestPreviousWeekday2(string dateText, int days, string expectedText) {
			var date = DateTime.Parse(dateText);
			var expected = DateTime.Parse(expectedText);
			var actual = date.PreviousWeekday(days);
			Assert.Equal(expected, actual);
		}

		/// <summary>
		/// the stupid way is actually faster when weekdayCount < 5
		/// </summary>
		public static DateTime PreviousWeekDayStupidWay(DateTime date, int weekdayCount) {
			date = date.Date;
			for (int i = 0; i < weekdayCount; i++) {
				date = date.AddDays(-1);
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					date = date.AddDays(-2);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					date = date.AddDays(-1);
				}
			}
			if (date.DayOfWeek == DayOfWeek.Sunday) {
				date = date.AddDays(-2);
			} else if (date.DayOfWeek == DayOfWeek.Saturday) {
				date = date.AddDays(-1);
			}
			return date;
		}
		public static DateTime NextWeekDayStupidWay(DateTime date, int weekdayCount) {
			date = date.Date;
			for (int i = 0; i < weekdayCount; i++) {
				date = date.AddDays(1);
				if (date.DayOfWeek == DayOfWeek.Sunday) {
					date = date.AddDays(1);
				} else if (date.DayOfWeek == DayOfWeek.Saturday) {
					date = date.AddDays(2);
				}
			}
			if (date.DayOfWeek == DayOfWeek.Sunday) {
				date = date.AddDays(1);
			} else if (date.DayOfWeek == DayOfWeek.Saturday) {
				date = date.AddDays(2);
			}
			return date;
		}
		[Fact]
		public async Task BlanketNextWeekDayTest() {
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 366 * 3; i++) {
				var date = new DateTime(2000, 1, 1).AddDays(i);
				for (int numberOfWeekDays = 0; numberOfWeekDays < 366 * 3; numberOfWeekDays++) {
					stopwatch.Restart();
					var actual = date.NextWeekday(numberOfWeekDays);

					var expected = NextWeekDayStupidWay(date, numberOfWeekDays);
					stopwatch.Restart();

					var actual2 = await date.NextBusinessDay(numberOfWeekDays);
					Assert.Equal(expected, actual);
					Assert.Equal(expected, actual2);
				}
			}
		}
		[Fact]
		public async Task BlanketPreviousWeekDayTest() {
			for (int i = 0; i < 366 * 3; i++) {
				var date = new DateTime(2000, 1, 1).AddDays(i);
				for (int numberOfWeekDays = 0; numberOfWeekDays < 366 * 3; numberOfWeekDays++) {
					var actual = date.PreviousWeekday(numberOfWeekDays);
					var expected = PreviousWeekDayStupidWay(date, numberOfWeekDays);
					var actual2 = await date.PreviousBusinessDay(numberOfWeekDays);
					Assert.Equal(expected, actual);
					Assert.Equal(expected, actual2);
				}
			}
		}

		[Theory]
		[InlineData("2023-10-01", 1, DayOfWeek.Wednesday, "2023-10-04")]
		[InlineData("2023-10-05", 1, DayOfWeek.Wednesday, "2023-10-11")]
		[InlineData("2023-10-05", 2, DayOfWeek.Wednesday, "2023-10-18")]
		[InlineData("2023-10-03", 2, DayOfWeek.Wednesday, "2023-10-11")]
		[InlineData("2023-10-02", 1, DayOfWeek.Monday, "2023-10-02")]
		[InlineData("2023-10-01", 1, DayOfWeek.Monday, "2023-10-02")]
		[InlineData("2023-10-03", 1, DayOfWeek.Monday, "2023-10-09")]
		public void TestGetNthDayOfWeek(string date, int n, DayOfWeek dayOfWeek, string expected) {
			var result = DateTime.Parse(date).GetNthDayOfWeek(n, dayOfWeek);
			Assert.Equal(DateTime.Parse(expected), result);
		}

		[Theory]
		[InlineData("2023-01-01", "2023-01-31", 0)]
		[InlineData("2023-01-01", "2023-02-01", 1)]
		[InlineData("2023-02-01", "2023-03-20", 1)]
		[InlineData("2023-02-01", "2023-01-20", -1)]
		[InlineData("2020-12-01", "2021-1-20", 1)]
		[InlineData("2021-01-01", "2020-12-20", -1)]
		public void TestGetMonthDiff(string date1Text, string date2Text, int expectedResult) {
			var result = DateTime.Parse(date1Text).GetMonthDiff(DateTime.Parse(date2Text));
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData("2024-01-01", "2024-01-01", 1)]
		[InlineData("2024-01-01", "2024-01-02", 2)]
		[InlineData("2024-01-02", "2024-01-01", 2)]
		[InlineData("2024-01-01", "2024-01-08", 6)]
		[InlineData("2024-01-08", "2024-01-01", 6)]
		[InlineData("2024-01-08", "2024-01-09", 2)]
		[InlineData("2024-01-08", "2024-01-05", 2)]
		[InlineData("2024-01-01", "2024-01-31", 23)]
		public void TestNumberOfWeekDays(string date1Text, string date2Text, int expectedResult) {
			var d1 = DateTime.Parse(date1Text);
			var d2 = DateTime.Parse(date2Text);
			var result = d1.GetNumberOfWeekdays(d2);
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData("2023-01-01", "2023-01-31")]
		[InlineData("2024-02-10", "2024-02-29")]
		public void TestEndOfMonth(string dateText, string expectedEndOfMonth) {
			var date = DateTime.Parse(dateText);
			var expected = DateTime.Parse(expectedEndOfMonth);
			var actual = date.EndOfMonth();
			Assert.Equal(expected, actual);
		}
	}
}
