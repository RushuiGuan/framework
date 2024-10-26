using Albatross.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Albatross.Serialization.Test {
	[JsonConverter(typeof(JsonEnumTextConverter<MyEnum>))]
	public enum MyEnum {
		[EnumText("wild cow")]
		Cow,

		[EnumText("small pig")]
		Pig
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum HisEnum {
		A,
		B
	}

	public class TestJsonConverter {
		[Theory]
		[InlineData(MyEnum.Cow, "\"wild cow\"")]
		[InlineData(MyEnum.Pig, "\"small pig\"")]
		public void TestSerialization(MyEnum value, string expected) {
			var text = JsonSerializer.Serialize(value);
			Assert.Equal(expected, text);
		}

		[Theory]
		[InlineData(MyEnum.Cow, "\"wild cow\"")]
		[InlineData(MyEnum.Pig, "\"small pig\"")]
		public void TestDeSerialization(MyEnum expected, string text) {
			var result = JsonSerializer.Deserialize<MyEnum>(text);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void Test() {
			JsonSerializer.Deserialize<HisEnum>("\"A\"");

		}
	}
}