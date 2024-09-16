using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class EqualStatementBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var error = $"{nameof(EqualStatementBuilder)} expected 2 parameters of type {nameof(ExpressionSyntax)}";
			var array = elements.ToArray();
			if (array.Length == 2) {
				if (array.First() is ExpressionSyntax first && array.Last() is ExpressionSyntax last) {
					return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, first, last);
				} else {
					throw new ArgumentException(error);
				}
			} else {
				throw new ArgumentException(error);
			}
		}
	}
}