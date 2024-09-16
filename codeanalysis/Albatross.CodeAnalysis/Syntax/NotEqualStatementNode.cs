using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Albatross.CodeAnalysis.Syntax {
	public class NotEqualStatementNode : NodeContainer {
		public NotEqualStatementNode(INodeContainer left, INodeContainer right) : base(Create(left, right)) {
		}
		static SyntaxNode Create(INodeContainer left, INodeContainer right) {
			if (left.Node is ExpressionSyntax syntax1 && right.Node is ExpressionSyntax syntax2) {
				return SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, syntax1, syntax2);
			} else {
				throw new ArgumentException($"{nameof(NotEqualStatementNode)} expects two expression syntax nodes");
			}
		}
	}
}