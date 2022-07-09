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
	}
}
