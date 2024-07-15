using Microsoft.CodeAnalysis;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestAttributeCheck {
		[Fact]
		public void TestGetGenericAttributeName() {
			var compilation = Extensions.CreateCompilation(@"
	using System;
	using System.Text.Json.Serialization;
	[JsonConverterAttribute(typeof(JsonStringEnumConverter))]
	public enum MyEnum1 { None }
	
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum MyEnum2 { None }
");
			var errors = compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
			if (errors.Any()) {
				errors.ToList().ForEach(x => System.Console.WriteLine(x));
			}
			var symbol = compilation.GetRequiredSymbol("MyEnum1");
			Assert.True(symbol.HasAttributeWithArguments("System.Text.Json.Serialization.JsonConverterAttribute", "System.Text.Json.Serialization.JsonStringEnumConverter"));
		}
	}
}
