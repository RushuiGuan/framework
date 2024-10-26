using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Albatross.CodeAnalysis.Syntax {
	public class StatementNode : NodeContainer {
		public StatementNode(SyntaxNode node) : base(Create(node)) { }

		public StatementSyntax StatementSyntax => (StatementSyntax)Node;

		static StatementSyntax Create(SyntaxNode element) {
			switch (element) {
				case StatementSyntax statementSyntax:
					return statementSyntax;
				case VariableDeclarationSyntax variableDeclarationSyntax:
					return SyntaxFactory.LocalDeclarationStatement(variableDeclarationSyntax);
				case ExpressionSyntax expressionSyntax:
					return SyntaxFactory.ExpressionStatement(expressionSyntax);
				default:
					throw new InvalidOperationException($"SyntaxNode of type {element.GetType().Name} cannot be converted to statement");
			}
		}
	}
}