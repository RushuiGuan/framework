using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Symbols {
	public class TestAttributeCheck {
		[Fact]
		public void TestGetGenericAttributeName() {
			var compilation = @"
	using System;
	using System.Text.Json.Serialization;
	[JsonConverterAttribute(typeof(JsonStringEnumConverter))]
	public enum MyEnum1 { None }
	
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum MyEnum2 { None }
".CreateCompilation();
			var errors = compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
			if (errors.Any()) {
				errors.ToList().ForEach(x => Console.WriteLine(x));
			}
			var symbol = compilation.GetRequiredSymbol("MyEnum1");
			Assert.True(symbol.HasAttributeWithArguments("System.Text.Json.Serialization.JsonConverterAttribute", "System.Text.Json.Serialization.JsonStringEnumConverter"));
		}
	}
}