using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestConstructorCreation {
		const string ClassBuilderWithConstructor_Expected = @"public class Test
{
	public Test()
	{
	}
}
";
		[Fact]
		public void SimpleConstructor() {
			var node = new CodeStack()
				.Begin(new ClassDeclarationBuilder("Test").Public())
					.Begin(new ConstructorDeclarationBuilder("Test").Public()).End()
				.End().Build();
			Assert.Equal(ClassBuilderWithConstructor_Expected, node.ToString());
		}
		const string ConstructorWithParameter_Expected = @"public class Test
{
	public Test(string name)
	{
	}
}
";

		[Fact]
		public void ConstructorWithParameter() {
			var node = new CodeStack()
				.Begin(new ClassDeclarationBuilder("Test").Public())
					.Begin(new ConstructorDeclarationBuilder("Test").Public()).With(new ParameterNode("string", "name"))
					.End()
				.End().Build();
			Assert.Equal(ConstructorWithParameter_Expected, node.ToString());
		}
		const string ConstructorWithParameterAndBaseCall_Expected = @"public class Test
{
	public Test(string name) : base(name)
	{
	}
}
";
		[Fact]
		public void ConstructorWithParameterAndBaseCall() {
			var node = new CodeStack()
				.Begin(new ClassDeclarationBuilder("Test").Public())
					.Begin(new ConstructorDeclarationBuilder("Test").Public()).With(new ParameterNode("string", "name"))
						.Begin(new ArgumentListBuilder()).With(new IdentifierNode("name")).End()
					.End()
				.End().Build();
			Assert.Equal(ConstructorWithParameterAndBaseCall_Expected, node.ToString());
		}
	}
}