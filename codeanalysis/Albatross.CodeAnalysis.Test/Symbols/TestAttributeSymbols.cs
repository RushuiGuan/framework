using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;
namespace Albatross.CodeAnalysis.Test.Symbols {
	public class TestAttributeSymbols {
		public const string AttributeCode = @"
	using System;
	public class MyAttribute: Attribute {
		public string Name { get; }
		public int Id{ get; }
		public MyAttribute(string name) {
			this.Name = name;
		}
		public MyAttribute(int id) {
			this.Id = id;
		}
	}
";
		[Theory]
		[InlineData(@"[My(""a"")]class MyClass{ string P1;}", "a", "System.String")]
		[InlineData(@"[My(1)]class MyClass{ string P1;}", 1, "System.Int32")]
		public void Run(string code, object expected, string type) {
			var compilation = (AttributeCode + code).CreateCompilation();
			var symbol = compilation.GetRequiredSymbol("MyClass");
			symbol.TryGetAttribute("MyAttribute", out var data);
			Assert.NotNull(data);
			data.ConstructorArguments.FirstOrDefault().Value.Should().Be(expected);
			Assert.IsType(Type.GetType(type)!, data.ConstructorArguments.First().Value);
		}
	}
}