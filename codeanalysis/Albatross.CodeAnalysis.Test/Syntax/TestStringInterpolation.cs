using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestStringInterpolation {
		[Fact]
		public void Simple() {
			var result = new InterpolatedStringBuilder().Build([new IdentifierNode("test").Node, new LiteralNode("x").Node]).ToFullString();
			result.Should().Be("$\"{test}x\"");
		}


		[Fact]
		public void Multiple() {
			var result = new InterpolatedStringBuilder().Build([
				new IdentifierNode("test").Node,
				new LiteralNode("x").Node,
				new IdentifierNode("test").Node,
				new LiteralNode("x").Node
			]).ToFullString();
			result.Should().Be("$\"{test}x{test}x\"");
		}

		[Fact]
		public void WithFormat() {
			var codeStack = new CodeStack();
			var node = new InterpolatedStringBuilder().Build([
				new LiteralNode("/").Node,
				new StringInterpolationBuilder("test", "yyyy-MM-dd").Build([]),
				new LiteralNode("x").Node,
				new LiteralNode("/").Node,
				new StringInterpolationBuilder("test", "#,#0").Build([]),
				new LiteralNode("/").Node,
				new LiteralNode("array").Node
			]);
			codeStack.With(node);
			codeStack.BuildWithFormat().Trim().Should().Be("$\"/{test:yyyy-MM-dd}x/{test:#,#0}/array\"");
		}
		[Fact]
		public void AllLiteral() {
			var codeStack = new CodeStack();
			using (codeStack.NewScope(new VariableBuilder("string", "path"))) {
				using (codeStack.NewScope(new InterpolatedStringBuilder())) {
					codeStack.With(new IdentifierNode("ControllerPath"))
						.With(new LiteralNode("/"))
						.With(new LiteralNode("array-string-param"));
				}
			}
			codeStack.BuildWithFormat().Trim().Should().Be(@"string path = $""{ControllerPath}/array-string-param""");
		}

		/// <summary>
		/// For whatever reason, the formatter will create a space after '/' literal in the interpolated string
		/// should ask the question in stack overflow,  Run this test to see if it is fixed
		/// </summary>
		[Fact(Skip = "this is a behavior test.  Run to see if changed")]
		public void LowLevelCheck() {
			using var workspace = new AdhocWorkspace();
			var interpolatedString = SyntaxFactory.InterpolatedStringExpression(
				SyntaxFactory.Token(SyntaxTriviaList.Empty,
					SyntaxKind.InterpolatedStringStartToken,
					SyntaxTriviaList.Empty),

					SyntaxFactory.List(new InterpolatedStringContentSyntax[] {
						SyntaxFactory.InterpolatedStringText(
							SyntaxFactory.Token(SyntaxTriviaList.Empty,
							SyntaxKind.InterpolatedStringTextToken, "/", "/",
							SyntaxTriviaList.Empty)
						),
						SyntaxFactory.InterpolatedStringText(
							SyntaxFactory.Token(SyntaxTriviaList.Empty,
							SyntaxKind.InterpolatedStringTextToken, "Hello", "Hello",
							SyntaxTriviaList.Empty)
						),
					}),
					SyntaxFactory.Token(SyntaxTriviaList.Empty,
						SyntaxKind.InterpolatedStringEndToken,
						SyntaxTriviaList.Empty)
			);
			var formatted = Formatter.Format(interpolatedString.NormalizeWhitespace(), workspace, null);
			formatted.ToFullString().Should().Be(@"$""/Hello""");
		}
	}
}