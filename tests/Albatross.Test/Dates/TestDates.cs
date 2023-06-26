using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Albatross.Dates;

namespace Albatross.Test.Dates {
	public class TestDates {
		
		[Theory]
		[InlineData("2022-07-09", "2022-07-08")]
		[InlineData("2022-07-10", "2022-07-08")]
		[InlineData("2022-07-11", "2022-07-08")]
		[InlineData("2022-07-08", "2022-07-07")]
		public void TestPreviousWeekday(string dateText, string expectedText) {
			var date = DateTime.Parse(dateText);
			var expected = DateTime.Parse(expectedText);
			var actual = date.PreviousWeekday();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("2022-07-08", "2022-07-11")]
		[InlineData("2022-07-09", "2022-07-11")]
		[InlineData("2022-07-10", "2022-07-11")]
		[InlineData("2022-07-07", "2022-07-08")]
		public void TestNextWeekday(string dateText, string expectedText) {
			var date = DateTime.Parse(dateText);
			var expected = DateTime.Parse(expectedText);
			var actual = date.NextWeekday();
			Assert.Equal(expected, actual);
		}

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
			Assert.True(date.Kind == DateTimeKind.Utc);
		}
	}
}
