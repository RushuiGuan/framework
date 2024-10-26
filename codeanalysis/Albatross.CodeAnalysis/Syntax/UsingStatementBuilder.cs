using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a <see cref="UsingStatementSyntax"/> instance.  
	/// Accept the first parameter of type <see cref="VariableDeclarationSyntax"/> or <see cref="ObjectCreationExpressionSyntax"/>
	/// <see cref="VariableDeclarationSyntax"/> can be used to create a new local variable with initialization for the using statement.
	/// <see cref="ObjectCreationExpressionSyntax"/> can be used to create a new object without declaring a variable.
	/// The rest of the parameters are the statements that are executed within the body of the using block.
	/// </summary>
	public class UsingStatementBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (!elements.Any()) {
				throw new ArgumentException("Using statement expects at least one parameter");
			}
			var node = elements.First();
			VariableDeclarationSyntax? declaration = null;
			ObjectCreationExpressionSyntax? objectCreation = null;
			if (node is VariableDeclarationSyntax variableDeclarationSyntax) {
				declaration = variableDeclarationSyntax;
			} else if (node is ObjectCreationExpressionSyntax objectCreationExpressionSyntax) {
				objectCreation = objectCreationExpressionSyntax;
			} else {
				throw new ArgumentException($"First parameter of using statement must be of type VariableDeclarationSyntax or ObjectCreationExpressionSyntax");
			}
			var statements = new List<StatementSyntax>();
			foreach (var element in elements.Skip(1)) {
				statements.Add(new StatementNode(element).StatementSyntax);
			}
			return SyntaxFactory.UsingStatement(declaration, objectCreation, SyntaxFactory.Block(statements));
		}
	}
}