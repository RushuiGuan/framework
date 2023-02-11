using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace Albatross.Test.Serialization {

	[JsonDerivedType(typeof(MyClass), "my-class")]
	[JsonPolymorphic]
	public class MyBase {
		public string Name { get; set; }
		public MyBase(string name) {
			this.Name = name;
		}
	}
	
	public class MyClass : MyBase {
		public string Type { get; set; }
		public MyClass(string name, string type) : base(name) {
			Type = type;
		}
	}

	public class TestPolymorphic {
		const string myClass = @"{""$type"":""my-class"",""Type"":""b"",""Name"":""a""}";
		const string myBase = @"{""Name"":""c""}";
		[Fact]
		public void TestSerialization() {
			MyBase dto = new MyClass("a", "b");
			var text = JsonSerializer.Serialize(dto);
			Assert.Equal(myClass, text);

			dto = new MyBase("c");
			text = JsonSerializer.Serialize(dto);
			Assert.Equal(@"{""Name"":""c""}", text);
		}
		[Fact]
		public void TestDeserialization() {
			var dto = JsonSerializer.Deserialize<MyBase>(myBase);
			Assert.IsType<MyBase>(dto);

			dto = JsonSerializer.Deserialize<MyBase>(myClass);
			Assert.IsType<MyClass>(dto);
		}
	}
}
