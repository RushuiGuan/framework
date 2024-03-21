using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Dates.Test.Dates {
	public class TestTimeZoneLogic {
		[Theory]
		[InlineData("Pacific Standard Time", "2024-03-09T22:00", -480)]
		[InlineData("Pacific Standard Time", "2024-03-09T22:59", -480)]
		[InlineData("Pacific Standard Time", "2024-03-09T23:00", -480)]
		[InlineData("Pacific Standard Time", "2024-03-10T02:00", -480)]
		[InlineData("Pacific Standard Time", "2024-03-10T02:59", -480)]
		[InlineData("Pacific Standard Time", "2024-03-10T03:00", -420)]

		[InlineData("Eastern Standard Time", "2024-01-01T12:00", -300)]
		[InlineData("Eastern Standard Time", "2024-03-09T23:59", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T00:00", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T01:00", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T02:00", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T02:59", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T03:00", -240)]
		[InlineData("Eastern Standard Time", "2024-03-10T06:00Z", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T06:59Z", -300)]
		[InlineData("Eastern Standard Time", "2024-03-10T07:00Z", -240)]
		[InlineData("Eastern Standard Time", "2024-03-10T08:00Z", -240)]
		public void TestTimeZoneOffsetWithDateTime(string timeZoneId, string dateTimeString, int expectedOffsetInMinutes) {
			DateTime dateTime = DateTime.Parse(dateTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind);
			TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			var offset = timeZone.GetUtcOffset(dateTime).TotalMinutes;
			Assert.Equal(expectedOffsetInMinutes, offset);
		}

		[Theory]
		[InlineData("Eastern Standard Time", "2024-03-09T22:00-8", -300)]
		[InlineData("Eastern Standard Time", "2024-03-09T22:59-8", -300)]
		[InlineData("Eastern Standard Time", "2024-03-09T23:00-8", -240)]
		public void TestTimeZoneOffsetWithDateTimeOffset(string timeZoneId, string dateTimeString, int expectedOffsetInMinutes) {
			DateTimeOffset dateTime = DateTimeOffset.Parse(dateTimeString);
			TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			var offset = timeZone.GetUtcOffset(dateTime).TotalMinutes;
			Assert.Equal(expectedOffsetInMinutes, offset);
		}

		[Theory]
		[InlineData("Eastern Standard Time", "2024-03-10T06:00", "2024-03-10T01:00-5")]
		[InlineData("Eastern Standard Time", "2024-03-10T06:59", "2024-03-10T01:59-5")]
		[InlineData("Eastern Standard Time", "2024-03-10T07:00", "2024-03-10T03:00-4")]
		[InlineData("Pacific Standard Time", "2024-03-10T07:00", "2024-03-09T23:00-8")]
		[InlineData("Central Standard Time", "2024-03-10T07:00", "2024-03-10T01:00-6")]
		public void TestUtc2DateTimeOffset(string timeZoneId, string dateTimeString, string expectedResult) {
			var utc = DateTime.Parse(dateTimeString);
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			var result = utc.Utc2DateTimeOffset(timeZone);
			Assert.Equal(expectedResult, $"{result:yyyy-MM-ddTHH:mmz}");
		}

		[Theory]
		[InlineData("Eastern Standard Time", "2024-03-10T01:00", "2024-03-10T01:00-5")]
		[InlineData("Eastern Standard Time", "2024-03-10T02:00", "2024-03-10T02:00-5")]
		[InlineData("Eastern Standard Time", "2024-03-10T02:59", "2024-03-10T02:59-5")]
		[InlineData("Eastern Standard Time", "2024-03-10T03:00", "2024-03-10T03:00-4")]
		public void TestLocal2DateTimeOffset(string timeZoneId, string dateTimeString, string expectedResult) {
			var local = DateTime.Parse(dateTimeString);
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			var result = local.Local2DateTimeOffset(timeZone);
			Assert.Equal(expectedResult, $"{result:yyyy-MM-ddTHH:mmz}");
		}
	}
}
