using Albatross.Reflection;
using Albatross.Serialization;
using System.Collections.Specialized;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Sdk;

namespace Albatross.Test.Serialization {
	public class ClassA {
		public	string Name { get; set; }
	}

	public class TestTypedValueSerialization{
		[Fact]
		public void TestUndefinedCases() {
			string text = "{}";
			var result = JsonSerializer.Deserialize<TypedValue>(text);
			Assert.Equal(typeof(JsonElement).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.True(((JsonElement)result.Value).ValueKind == JsonValueKind.Undefined);
		}

		[Fact]
		public void TestNullCases() {
			string text = this.GetType().GetEmbeddedFile("TypedValue.NullValue.json");
			var result = JsonSerializer.Deserialize<TypedValue>(text);
			Assert.Equal(typeof(JsonElement).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.True(((JsonElement)result.Value).ValueKind == JsonValueKind.Null);
		}

		[Fact]
		public void TestNormalCase() {
			string text = this.GetType().GetEmbeddedFile("TypedValue.Normal.json");
			var result = JsonSerializer.Deserialize<TypedValue>(text);
			Assert.Equal(typeof(ClassA).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.IsType<ClassA>(result.Value);
			Assert.Equal("test", ((ClassA)result.Value).Name);
		}

		[Fact]
		public void TestOutOfOrderCase() {
			string text = this.GetType().GetEmbeddedFile("TypedValue.OutOfOrder.json");
			var result = JsonSerializer.Deserialize<TypedValue>(text);
			Assert.Equal(typeof(ClassA).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.IsType<ClassA>(result.Value);
			Assert.Equal("test", ((ClassA)result.Value).Name);
		}

		[Fact]
		public void TestSomeSpecialCases() {
			TypedValue value = new TypedValue {
				ClassName = typeof(WebApiParam).GetTypeNameWithoutAssemblyVersion(),
				Value = new WebApiParam (),
			};
			string result = JsonSerializer.Serialize<TypedValue>(value);
			Assert.NotNull(result);
		}

		[Fact]
		public void TestNullDeserialization() {
			string text = "{\"className\" : \"Albatross.Test.Serialization.WebApiParam, Albatross.Test\"}";
			TypedValue value  = JsonSerializer.Deserialize<TypedValue>(text, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
			Assert.Equal(typeof(WebApiParam).GetTypeNameWithoutAssemblyVersion(), value.ClassName);
			Assert.Null(value.Value);
		}

	}

	public class WebApiParam {
		public JsonElement FromBody { get; set; }
		public NameValueCollection FromQuery { get; set; }
		public NameValueCollection FromHeader { get; set; }
		public NameValueCollection FromForm { get; set; }
		public string ContentType { get; set; }
	}
}
