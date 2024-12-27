using Albatross.Authentication.AspNetCore;
using Xunit;

namespace Albatross.Authentication.UnitTest {
	public class TestAspNetCoreAuthentication {
		[Theory]
		[InlineData("domain\\user", "user")]
		[InlineData("user", "user")]
		[InlineData("", My.Anonymous)]
		[InlineData(null, My.Anonymous)]
		public void TestNormalizeIdentity(string? name, string expected) {
			var result = name.NormalizeIdentity();
			Assert.Equal(expected, result);
		}
	}
}