using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class IfStatementBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.FirstOrDefault() is ExpressionSyntax expressionSyntax) {
				var statements = elements.Skip(1);
				return SyntaxFactory.IfStatement(expressionSyntax, statements.BlockSyntax());
			} else {
				throw new ArgumentException($"{nameof(IfStatementBuilder)} expects an {nameof(ExpressionSyntax)} as its condition");
			}
		}
	}
}