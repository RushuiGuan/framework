using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class IdentifierNode : NodeContainer {
		public IdentifierNode(params string[] names) : base(Create(false, names)) {
		}
		public IdentifierNode(bool memberAccess, params string[] names) : base(Create(memberAccess, names)) { }

		public ExpressionSyntax Identifier => (ExpressionSyntax)this.Node;

		static ExpressionSyntax Create(bool memberAccess, params string[] names) {
			ExpressionSyntax expressionSyntax = null;
			if (memberAccess) {
				expressionSyntax = SyntaxFactory.ThisExpression();
			}
			foreach (var name in names) {
				if (expressionSyntax == null) {
					expressionSyntax = SyntaxFactory.IdentifierName(name);
				} else {
					expressionSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, SyntaxFactory.IdentifierName(name));
				}
			}
			return expressionSyntax ?? throw new ArgumentException("Set memberAccess flag to true or have at least one names");
		}
	}
}
