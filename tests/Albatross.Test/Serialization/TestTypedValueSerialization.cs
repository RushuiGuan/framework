using Albatross.Reflection;
using Albatross.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

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
	}
}
