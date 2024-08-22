using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	/// <summary>
	/// Create an <see cref="AssignmentExpressionSyntax">.  Expects one parameter of type <see cref="ExpressionSyntax"/>
	/// ```
	/// a = 1
	/// ```
	/// </summary>
	public class Assignment : INodeBuilder {
		public Assignment(string identifierName) {
			IdentifierName = identifierName;
		}

		public string IdentifierName { get; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Count() == 1 && elements.First() is ExpressionSyntax expressionSyntax) {
				return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
					SyntaxFactory.IdentifierName(this.IdentifierName),
					expressionSyntax);
			} else {
				throw new ArgumentException($"Assignment operations expects one parameter of type ExpressionSyntax");
			}
		}
	}

}
