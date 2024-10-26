using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class ReturnExpressionBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.FirstOrDefault() is ExpressionSyntax expression) {
				return SyntaxFactory.ReturnStatement(expression);
			} else {
				throw new ArgumentException($"{nameof(ReturnExpressionBuilder)} expects a single {nameof(ExpressionSyntax)} parameter");
			}
		}
	}
}