using System;
using Xunit;

namespace Albatross.Dates.Test.Dates {
	public class TestDateTimeConversion {

		[Fact]
		public void TestLocal2Utc() {
			var gmtOffset = TimeSpan.FromMinutes(60);
			var datetime = new DateTime(2021, 1, 1);
			var utc = datetime.Local2Utc(gmtOffset);
			Assert.Equal(DateTimeKind.Utc, utc.Kind);
			Assert.Equal(utc.Utc2Local(gmtOffset), datetime);

			Assert.Equal(utc.GmtOffset(datetime), gmtOffset);

			var datetimeoffset = new DateTimeOffset(datetime, gmtOffset);
			// the datetimeoffset object has the same time as the utc object
			Assert.Equal(datetimeoffset, utc);
			// they have the same file time and are representing the same time
			Assert.Equal(datetimeoffset.ToFileTime(), utc.ToFileTime());
			// for some reason they have diff ticks
			Assert.NotEqual(datetimeoffset.Ticks, utc.Ticks);
		}
	}
}
