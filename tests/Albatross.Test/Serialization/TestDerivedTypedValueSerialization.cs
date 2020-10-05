using Albatross.Reflection;
using Albatross.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Albatross.Test.Serialization {
	[JsonConverter(typeof(MyTypedValueJsonConverter))]
	public class MyTypedValue : TypedValue {
		public MyTypedValue() {
			ClassName = typeof(JsonElement).GetTypeNameWithoutAssemblyVersion();
		}
		public string Description { get; set; }
	}

	public class MyTypedValueJsonConverter: TypedValueJsonConverter<MyTypedValue> {
		protected override void ReadAdditionalProperty(ref Utf8JsonReader reader, MyTypedValue artifact, string propertyName, JsonSerializerOptions options) {
			if(CheckPropertyName(nameof(MyTypedValue.Description), propertyName, options)) {
				if (reader.Read()) {
					artifact.Description = reader.GetString();
				}
			}
		}
		protected override void WriteAdditionalProperty(Utf8JsonWriter writer, MyTypedValue value, JsonSerializerOptions options) {
			writer.WriteString(GetPropertyName(nameof(MyTypedValue.Description), options), value.Description);
		}
	}


	public class TestDerivedTypedValueSerialization {
		[Fact]
		public void TestDerivedClass_Normal() {
			string text = this.GetType().GetEmbeddedFile("MyTypedValue.Normal.json");
			var result = JsonSerializer.Deserialize<MyTypedValue>(text);
			Assert.Equal(typeof(ClassA).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.IsType<ClassA>(result.Value);
			Assert.Equal("test", ((ClassA)result.Value).Name);
			Assert.Equal("desc", result.Description);
		}

		[Fact]
		public void TestDerivedClass_OutofOrder() { 
			string text = this.GetType().GetEmbeddedFile("MyTypedValue.OutOfOrder.json");
			var result = JsonSerializer.Deserialize<MyTypedValue>(text);
			Assert.Equal(typeof(ClassA).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.IsType<ClassA>(result.Value);
			Assert.Equal("test", ((ClassA)result.Value).Name);
			Assert.Equal("desc", result.Description);
		}

		[Fact]
		public void TestDerivedClass_NullValue() {
			string text = this.GetType().GetEmbeddedFile("MyTypedValue.NullValue.json");
			var result = JsonSerializer.Deserialize<MyTypedValue>(text);
			Assert.Equal(typeof(JsonElement).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.True(((JsonElement)result.Value).ValueKind == JsonValueKind.Null);
			Assert.Equal("desc", result.Description);
		}

		[Fact]
		public void TestDerivedClass_Undefined() {
			string text = this.GetType().GetEmbeddedFile("MyTypedValue.Undefined.json");
			var result = JsonSerializer.Deserialize<MyTypedValue>(text);
			Assert.Equal(typeof(JsonElement).GetTypeNameWithoutAssemblyVersion(), result.ClassName);
			Assert.IsType<JsonElement>(result.Value);
			Assert.True(((JsonElement)result.Value).ValueKind == JsonValueKind.Undefined);
			Assert.Equal("desc", result.Description);
		}
	}
}
