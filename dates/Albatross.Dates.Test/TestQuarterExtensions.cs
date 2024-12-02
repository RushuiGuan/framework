using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Dates.Test {
	public class TestQuarterExtensions {
		[Theory]
		[InlineData("2021-01-01", 1)]
		[InlineData("2021-02-01", 1)]
		[InlineData("2021-03-01", 1)]
		[InlineData("2021-04-01", 2)]
		[InlineData("2021-05-01", 2)]
		[InlineData("2021-06-01", 2)]
		[InlineData("2021-07-01", 3)]
		[InlineData("2021-08-01", 3)]
		[InlineData("2021-09-01", 3)]
		[InlineData("2021-10-01", 4)]
		[InlineData("2021-11-01", 4)]
		[InlineData("2021-12-01", 4)]
		public void TestQuarter(string date, int quarter) {
			DateOnly d = DateOnly.Parse(date);
			Assert.Equal(quarter, d.Quarter());
		}

		[Theory]
		[InlineData("2021-01-01", "2021-01-01")]
		[InlineData("2021-02-01", "2021-01-01")]
		[InlineData("2021-03-01", "2021-01-01")]
		[InlineData("2021-04-01", "2021-04-01")]
		[InlineData("2021-05-01", "2021-04-01")]
		[InlineData("2021-06-01", "2021-04-01")]
		[InlineData("2021-07-01", "2021-07-01")]
		[InlineData("2021-08-01", "2021-07-01")]
		[InlineData("2021-09-01", "2021-07-01")]
		[InlineData("2021-10-01", "2021-10-01")]
		[InlineData("2021-11-01", "2021-10-01")]
		[InlineData("2021-12-01", "2021-10-01")]
		public void TestFirstMonthOfQuarter(string date, string firstMonth) {
			DateOnly d = DateOnly.Parse(date);
			DateOnly f = DateOnly.Parse(firstMonth);
			Assert.Equal(f, d.FirstMonthOfQuarter());
		}

		[Theory]
		[InlineData("2021-01-01", "2021-03-01")]
		[InlineData("2021-02-01", "2021-03-01")]
		[InlineData("2021-03-01", "2021-03-01")]
		[InlineData("2021-04-01", "2021-06-01")]
		[InlineData("2021-05-01", "2021-06-01")]
		[InlineData("2021-06-01", "2021-06-01")]
		[InlineData("2021-07-01", "2021-09-01")]
		[InlineData("2021-08-01", "2021-09-01")]
		[InlineData("2021-09-01", "2021-09-01")]
		[InlineData("2021-10-01", "2021-12-01")]
		[InlineData("2021-11-01", "2021-12-01")]
		[InlineData("2021-12-01", "2021-12-01")]
		public void TestLastMonthOfQuarter(string date, string lastMonth) {
			DateOnly d = DateOnly.Parse(date);
			DateOnly f = DateOnly.Parse(lastMonth);
			Assert.Equal(f, d.LastMonthOfQuarter());
		}

		[Theory]
		[InlineData("2021-01-01", "2021-03-31")]
		[InlineData("2021-02-01", "2021-03-31")]
		[InlineData("2021-03-01", "2021-03-31")]
		[InlineData("2021-04-01", "2021-06-30")]
		[InlineData("2021-05-01", "2021-06-30")]
		[InlineData("2021-06-01", "2021-06-30")]
		[InlineData("2021-07-01", "2021-09-30")]
		[InlineData("2021-08-01", "2021-09-30")]
		[InlineData("2021-09-01", "2021-09-30")]
		[InlineData("2021-10-01", "2021-12-31")]
		[InlineData("2021-11-01", "2021-12-31")]
		[InlineData("2021-12-01", "2021-12-31")]
		public void TestLastDateOfQuarter(string date, string lastDate) {
			DateOnly d = DateOnly.Parse(date);
			DateOnly f = DateOnly.Parse(lastDate);
			Assert.Equal(f, d.LastDateOfQuarter());
		}

		[Theory]
		[InlineData("2021-01-01", false)]
		[InlineData("2021-02-01", false)]
		[InlineData("2021-03-01", true)]
		[InlineData("2021-04-01", false)]
		[InlineData("2021-05-01", false)]
		[InlineData("2021-06-01", true)]
		[InlineData("2021-07-01", false)]
		[InlineData("2021-08-01", false)]
		[InlineData("2021-09-01", true)]
		[InlineData("2021-10-01", false)]
		[InlineData("2021-11-01", false)]
		[InlineData("2021-12-01", true)]
		public void TestIsLastMonthOfQuarter(string date, bool expected){
			DateOnly d = DateOnly.Parse(date);
			Assert.Equal(expected, d.IsLastMonthOfQuarter());
		}
	}
}
