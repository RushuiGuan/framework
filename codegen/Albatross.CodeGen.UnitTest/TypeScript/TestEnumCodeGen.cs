
using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Models;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Xunit;

namespace Albatross.CodeGen.UnitTest.TypeScript {
	public enum MyEnum {
		New, Brand, Best
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum MyEnum1 {
		New, Brand, Best
	}
	public class TestEnumCodeGen {
		[Fact]
		public void TestStringEnumValue() {
			var model = new ConvertEnumToTypeScriptEnum().Convert(typeof(MyEnum));
			StringWriter writer = new StringWriter();
			model.Generate(writer);
			string text = writer.ToString();


			model = new ConvertEnumToTypeScriptEnum().Convert(typeof(MyEnum1));
			writer = new StringWriter();
			model.Generate(writer);
			text = writer.ToString();

		}
	}
}
