using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	public class MemberAccessBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			ExpressionSyntax? expressionSyntax = null;
			foreach (var elem in elements) {
				if (expressionSyntax == null) {
					if (elem is ExpressionSyntax identifier) {
						expressionSyntax = identifier;
					} else {
						throw new ArgumentException($"The {nameof(MemberAccessBuilder)} only accepts {nameof(ExpressionSyntax)} as its first parameter");
					}
				} else if (elem is SimpleNameSyntax simpleNameSyntax) {
					expressionSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, simpleNameSyntax);
				} else {
					throw new ArgumentException($"The {nameof(MemberAccessBuilder)} only accepts {nameof(SimpleNameSyntax)} after its first parameter");
				}
			}
			if (expressionSyntax == null) {
				throw new ArgumentException($"The {nameof(MemberAccessBuilder)} requires at least one {nameof(ExpressionSyntax)} parameter");
			} else {
				return expressionSyntax;
			}
		}
	}
}