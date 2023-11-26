using System.Text.Json;
using Xunit;

namespace Albatross.Serialization.Test {
	public class TestApplyJsonValue {
		[Theory]
		 [InlineData("0", "1", "1")]
		 [InlineData("{}", "{}", "{}")]
		 [InlineData("[1]", "[2]", "[2]")]
		 [InlineData("[1]", "2", "2")]
		 [InlineData("{\"a\":1}", "false", "false")]
		[InlineData("{\"a\":1}", "{\"a\":2}", "{\"a\":2}")]
		[InlineData("{\"a\":1}", "{\"A\":2}", "{\"a\":1,\"A\":2}")]
		[InlineData("{\"a\":1}", "{\"b\":2}", "{\"a\":1,\"b\":2}")]
		[InlineData("{\"a\":1}", "{\"B\":2}", "{\"a\":1,\"B\":2}")]
		[InlineData("{\"a\":1}", "{\"a\":[1,2,3]}", "{\"a\":[1,2,3]}")]
		public void RunTestCases(string src, string value, string expected) {
			var srcValue = JsonSerializer.Deserialize<JsonElement>(src);
			var overrideValue = JsonSerializer.Deserialize<JsonElement>(value);
			var result = Albatross.Serialization.Extensions.ApplyJsonValue(srcValue, overrideValue);
			string text = JsonSerializer.Serialize(result);
			Assert.Equal(expected, text);
		}

		[Fact]
		public void TestNull() {
			JsonElement element = JsonSerializer.Deserialize<JsonElement>("null");
			Assert.Equal(JsonValueKind.Null, element.ValueKind);
		}
	}
}
