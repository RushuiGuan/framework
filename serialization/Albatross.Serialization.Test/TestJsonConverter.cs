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
	public class Test {
		public SecurityTypeText TextType { get; set; }
		public SecurityTypeInt IntType { get; set; }
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

		[Fact]
		public void TestEnumUnknownTextValueDeserialization() {
			var text = "{\"textType\":\"xxx\"}";
			var obj = System.Text.Json.JsonSerializer.Deserialize<Test>(text, CustomJsonSettings.Value.Default);
			Assert.Equal(SecurityTypeText.Undefined, obj?.TextType);
		}

		[Fact]
		public void TestEnumUnknownValueValueDeserialization() {
			var text = "{\"intType\": 99}";
			var obj = JsonSerializer.Deserialize<Test>(text, CustomJsonSettings.Value.Default);
			Assert.Equal(99, (int)obj.IntType);
		}

		[Fact]
		public void TestEnumValueSerialization() {
			var test = new Test { TextType = SecurityTypeText.Equity };
			var actual = System.Text.Json.JsonSerializer.Serialize<Test>(test, DefaultJsonSettings.Value.Default);
			var expected = System.Text.Json.JsonSerializer.Serialize<Test>(test, CustomJsonSettings.Value.Default);
			Assert.Equal(expected, actual);
		}
	}
}