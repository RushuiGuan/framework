using Albatross.Reflection;
using System.Text.Json;
using Xunit;

namespace Albatross.Serialization.Test {
	public class TestClass {
		public TestClass(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public string[]? Values { get; set; }
	}
	public class TestDefaultValue {
		[Fact]
		public void TestDefaultOptions() {
			var option = new JsonSerializerOptions() {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				 DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
			};
			var value = new TestClass("abc");
			var text = JsonSerializer.Serialize(value, option);
		}
	}
}
