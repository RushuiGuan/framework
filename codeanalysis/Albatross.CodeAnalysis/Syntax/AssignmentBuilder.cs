using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create an <see cref="AssignmentExpressionSyntax">.  Expects one parameter of type <see cref="ExpressionSyntax"/>
	/// ```
	/// a = 1
	/// ```
	/// </summary>
	public class AssignmentExpressionBuilder : INodeBuilder {
		public AssignmentExpressionBuilder(string name) : this(new IdentifierNode(name)) { }
		public AssignmentExpressionBuilder(IdentifierNode identifier) {
			IdentifierName = identifier.Identifier;
		}

		public ExpressionSyntax IdentifierName { get; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Count() == 1 && elements.First() is ExpressionSyntax expressionSyntax) {
				return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName, expressionSyntax);
			} else {
				throw new ArgumentException($"Assignment operations expects one parameter of type ExpressionSyntax");
			}
		}
	}
}
