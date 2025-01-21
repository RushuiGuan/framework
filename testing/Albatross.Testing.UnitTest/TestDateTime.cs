using Albatross.Dates;
using Xunit;

namespace Albatross.Testing.UnitTest {
	public class TestDateTime {
		[Theory]
		[InlineData("2021-01-01", "2021-01-01T00:00:00")]
		[InlineData("2021-01-01Z", "2021-01-01T00:00:00Z")]
		[InlineData("2021-01-01T00:00Z", "2021-01-01T00:00:00Z")]
		public void TestConvertToDateTime(string text, string expected) {
			var result = text.ConvertToDateTime().ISO8601String();
			Assert.Equal(expected, result);
		}
	}
}
