using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Xunit;
using Microsoft.CodeAnalysis.Formatting;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestStringInterpolation {
		[Fact]
		public void Simple() {
			var result = new StringInterpolationBuilder().Build([new IdentifierNode("test").Node, new LiteralNode("x").Node]).ToFullString();
			result.Should().Be("$\"{test}x\"");
		}


		[Fact]
		public void Multiple() {
			var result = new StringInterpolationBuilder().Build([
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
			var node = new StringInterpolationBuilder().Build([
				new StringInterpolationNode("test", "yyyy-MM-dd").Node,
				new LiteralNode("x").Node,
				new StringInterpolationNode("test", "#,#0").Node,
				new LiteralNode("/").Node,
				new LiteralNode("array").Node
			]);
			codeStack.With(node);
			codeStack.BuildWithFormat().Trim().Should().Be("$\"{test:yyyy-MM-dd}x{test:#,#0}/array\"");
		}
		[Fact]
		public void AllLiteral() {
			var codeStack = new CodeStack();
			using (codeStack.NewScope(new VariableBuilder("string", "path"))) {
				using (codeStack.NewScope(new StringInterpolationBuilder())) {
					codeStack.With(new IdentifierNode("ControllerPath"))
						.With(new LiteralNode("/"))
						.With(new LiteralNode("array-string-param"));
				}
			}
			codeStack.BuildWithFormat().Trim().Should().Be(@"string path = $""{ControllerPath}/array-string-param""");
		}

		[Fact]
		public void LowLevelCheck() {
			using var workspace = new AdhocWorkspace();
			var options = workspace.Options
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForElse, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForCatch, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForFinally, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInObjectInit, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForClausesInQuery, false)
				.WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, true)
				.WithChangedOption(FormattingOptions.TabSize, LanguageNames.CSharp, 4);
			var interpolatedString = SyntaxFactory.InterpolatedStringExpression(
				SyntaxFactory.Token(SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker), SyntaxKind.InterpolatedStringStartToken, SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker)),
				SyntaxFactory.List(new InterpolatedStringContentSyntax[] {
					SyntaxFactory.Interpolation(SyntaxFactory.IdentifierName("name")),
					SyntaxFactory.InterpolatedStringText(
						SyntaxFactory.Token(SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker), SyntaxKind.InterpolatedStringTextToken, "/", "/", SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker))
					),
					SyntaxFactory.InterpolatedStringText(
						SyntaxFactory.Token(SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker), SyntaxKind.InterpolatedStringTextToken, "Hello", "Hello", SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker))
					),
				}),
				SyntaxFactory.Token(SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker), SyntaxKind.InterpolatedRawStringEndToken, SyntaxTriviaList.Create(SyntaxFactory.ElasticMarker))
			);
			var formatted = Formatter.Format(interpolatedString, workspace, options);
			formatted.ToFullString().Should().Be("$\"{name}/Hello\"");
		}
	}
}

