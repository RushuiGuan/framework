using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a <see cref="ConstructorDeclarationSyntax"/>.  Expects:
	/// * <see cref="ParameterSyntax"/> - zero or more optional parameters for the constructor parameters
	/// * <see cref="ArgumentListSyntax"/> - argument list for the base constructor call
	/// * <see cref="StatementSyntax"/> - zero or more statements for the constructor body
	/// </summary>
	public class ConstructorDeclarationBuilder : INodeBuilder {
		public ConstructorDeclarationBuilder(string className) {
			Node = SyntaxFactory.ConstructorDeclaration(className);
		}
		public ConstructorDeclarationBuilder Public() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public ConstructorDeclarationBuilder Private() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
			return this;
		}
		public ConstructorDeclarationBuilder Protected() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
			return this;
		}
		public ConstructorDeclarationSyntax Node { get; private set; }


		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var statements = new List<StatementSyntax>();
			foreach (var element in elements) {
				if (element is ParameterSyntax parameter) {
					Node = Node.AddParameterListParameters(parameter);
				} else if (element is ArgumentListSyntax argumentList) {
					Node = Node.WithInitializer(SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, argumentList));
				} else {
					statements.Add(new StatementNode(element).StatementSyntax);
				}
			}
			Node = Node.WithBody(SyntaxFactory.Block(statements));
			return Node;
		}
	}
}