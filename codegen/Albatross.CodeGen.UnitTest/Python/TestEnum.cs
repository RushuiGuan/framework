using Albatross.CodeGen.Python.Conversions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Linq;
using Xunit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Albatross.CodeGen.Python.Models;

namespace Albatross.CodeGen.UnitTest.Python {
	public class TestEnum {
		[Fact]
		public void Run() {
			var code = @"public enum MyEnum {
			Apple,
			Orange,
			Banana,
		}";
			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
			CSharpCompilation compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(syntaxTree);
			SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
			var converter = new ConvertCSharpEnumDeclarationSyntaxToClassDeclaration(semanticModel);
			var result = converter.Convert(syntaxTree.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>().First());
			Assert.Equal("MyEnum", result.Name);
			Assert.Collection(result,
				x => {
					Assert.Equal("Apple", x.Name);
					var literal = x as Literal;
					Assert.NotNull(literal);
					Assert.Equal(0, literal.value);
				},
				x => {
					Assert.Equal("Orange", x.Name);
					var literal = x as Literal;
					Assert.NotNull(literal);
					Assert.Equal(1, literal.value);
				},
				x => {
					Assert.Equal("Banana", x.Name);
					var literal = x as Literal;
					Assert.NotNull(literal);
					Assert.Equal(2, literal.value);
				}
			);
		}
	}
}
