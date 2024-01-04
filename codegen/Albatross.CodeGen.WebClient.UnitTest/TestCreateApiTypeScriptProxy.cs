using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class TestCreateApiTypeScriptProxy {
		[Theory]
		[InlineData("TestService", "test.service.ts")]
		[InlineData("TestMyService", "test-my.service.ts")]
		[InlineData("AaBbCcService", "aa-bb-cc.service.ts")]
		[InlineData("ABCcService", "ab-cc.service.ts")]
		[InlineData("GDPService", "gdp.service.ts")]
		[InlineData("BBApiService", "bb-api.service.ts")]
		[InlineData("CUSIPDataService", "cusip-data.service.ts")]
		public void TestGetApiFileName(string name, string expected) {
			var converter = new CreateApiTypeScriptProxy(null!, null!);
			Assert.Equal(expected, converter.GetApiFileName(name));
		}
	}
}
