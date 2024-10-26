using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create an <see cref="AssignmentExpressionSyntax">.  
	/// Expects at least 2 <see cref="ExpressionSyntax"/> nodes.  The first n-1 nodes are used to create the left side of the assignment, 
	/// the last node is used to create the right side of the assignment.  The last node on the left side of the assignment can also be passed in
	/// as a parameter in the constructor.
	/// ```
	/// a = 1
	/// this.test.a = 1
	/// ```
	/// </summary>
	public class AssignmentExpressionBuilder : INodeBuilder {
		public AssignmentExpressionBuilder() { }
		public AssignmentExpressionBuilder(string name) : this(new IdentifierNode(name)) { }
		public AssignmentExpressionBuilder(IdentifierNode identifier) {
			IdentifierName = identifier.Identifier;
		}

		public ExpressionSyntax? IdentifierName { get; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var nameParameters = new List<SyntaxNode>();
			var array = elements.ToArray();
			// add all the elements except the last one
			for (int i = 0; i < array.Length - 1; i++) {
				nameParameters.Add(array[i]);
			}
			if (IdentifierName != null) {
				nameParameters.Add(IdentifierName);
			}
			if (nameParameters.Count == 0 || array.Length == 0) {
				throw new ArgumentException($"Assignment operations expects at least two parameter of type ExpressionSyntax");
			} else if (elements.Last() is ExpressionSyntax expressionSyntax) {
				var name = (ExpressionSyntax)new MemberAccessBuilder().Build(nameParameters);
				return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, name, expressionSyntax);
			} else {
				throw new ArgumentException($"Assignment operations only expects parameters of type ExpressionSyntax");
			}
		}
	}
}