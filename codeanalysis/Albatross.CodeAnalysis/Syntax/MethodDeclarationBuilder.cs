using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a <see cref="MethodDeclarationSyntax"/>.  Expects:
	/// * <see cref="ParameterSyntax"/> - zero or more optional parameters for the method parameters
	/// * <see cref="StatementSyntax"/> - zero or more statements for the method body
	/// </summary>
	public class MethodDeclarationBuilder : INodeBuilder {
		public MethodDeclarationBuilder(TypeNode returnType, string methodName) {
			Node = SyntaxFactory.MethodDeclaration(returnType.Type, methodName);
		}
		public MethodDeclarationBuilder(string returnType, string methodName) : this(new TypeNode(returnType), methodName) { }
		public MethodDeclarationBuilder Public() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public MethodDeclarationBuilder Private() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
			return this;
		}
		public MethodDeclarationBuilder Protected() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
			return this;
		}
		public MethodDeclarationBuilder Static() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
			return this;
		}
		public MethodDeclarationBuilder Async() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
			return this;
		}
		public MethodDeclarationSyntax Node { get; private set; }
		bool usedByInterface;
		public MethodDeclarationBuilder UsedByInterface() {
			usedByInterface = true;
			return this;
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var statements = new List<StatementSyntax>();
			foreach (var element in elements) {
				if (element is ParameterSyntax parameter) {
					Node = Node.AddParameterListParameters(parameter);
				} else {
					statements.Add(new StatementNode(element).StatementSyntax);
				}
			}
			if (usedByInterface) {
				Node = Node.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
			} else {
				Node = Node.WithBody(SyntaxFactory.Block(statements));
			}
			return Node;
		}
	}
}