using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Accept a mixture of <see cref="ExpressionSyntax"/>, <see cref="LiteralExpressionSyntax"/> and <see cref="InterpolationSyntax"/>
	/// </summary>
	public class StringInterpolationBuilder : INodeBuilder {
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
