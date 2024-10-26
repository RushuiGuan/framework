using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// This builder will create an interpolated string from a list of expressions, literals and interpolations
	/// For example:  $"{expression1} literal_text {number:#,#0}".  The last part of the example {number:#,#0} requires the use of
	/// <see cref="InterpolationSyntax"/> and can be created using <see cref="StringInterpolationNode"/> or <see cref="StringInterpolationBuilder"/>
	/// Accept a mixture of <see cref="ExpressionSyntax"/>, <see cref="LiteralExpressionSyntax"/> and <see cref="InterpolationSyntax"/>
	/// </summary>
	public class InterpolatedStringBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var list = new List<InterpolatedStringContentSyntax>();
			string text = string.Empty;
			foreach (var elem in elements) {
				if (elem is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression) {
					text = text + literal.Token.ValueText;
					continue;
				} else if (!string.IsNullOrEmpty(text)) {
					list.Add(SyntaxFactory.InterpolatedStringText(
						SyntaxFactory.Token(SyntaxTriviaList.Empty,
							SyntaxKind.InterpolatedStringTextToken,
							text,
							text,
							SyntaxTriviaList.Empty)
						)
					);
					text = string.Empty;
				}
				if (elem is ExpressionSyntax identifier) {
					list.Add(SyntaxFactory.Interpolation(identifier));
				} else if (elem is InterpolationSyntax interpolationSyntax) {
					list.Add(interpolationSyntax);
				} else {
					throw new ArgumentException($"Invalid element type {elem.GetType().Name}");
				}
			}
			if (!string.IsNullOrEmpty(text)) {
				list.Add(SyntaxFactory.InterpolatedStringText(
						SyntaxFactory.Token(SyntaxTriviaList.Empty,
							SyntaxKind.InterpolatedStringTextToken,
							text,
							text,
							SyntaxTriviaList.Empty)
						)
					);
			}
			return SyntaxFactory.InterpolatedStringExpression(SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
				.WithContents(SyntaxFactory.List(list));
		}
	}
}