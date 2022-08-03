using Albatross.CodeGen.TypeScript.Conversion;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public interface TestInterface<T> {
		T Name { get; set; }
	}
	public class TestTypeScriptGenericTypeCodeGen {
		[Fact]
		public void CreateGenericType() {
			var type = new TypeScriptType("Test");
			type.IsGeneric = true;
			type.GenericTypeArguments = new TypeScriptType[] {TypeScriptType.String() };
			StringWriter writer = new StringWriter();
			type.Generate(writer);
			string result = writer.ToString();
			Assert.Equal("Test<string>", result);
		}

		[Fact]
		public void CreateGenericInterface() {
			var converter = new ConvertTypeToTypeScriptInterface(new ConvertPropertyInfoToTypeScriptProperty(new ConvertTypeToTypeScriptType()), new ConvertTypeToTypeScriptType());
			var model = converter.Convert(typeof(TestInterface<>));
			Assert.Equal("TestInterface_", model.Name);
			var writer = new StringWriter();
			model.Generate(writer);
			string result= writer.ToString();
			Assert.Equal(@"export interface TestInterface_<T> {
	name: T;
}
", result);
		}
	}
}
