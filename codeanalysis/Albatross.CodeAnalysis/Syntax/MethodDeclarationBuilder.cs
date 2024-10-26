using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a <see cref="MethodDeclarationSyntax"/>.  Expects:
	/// * <see cref="ParameterSyntax"/> - zero or more optional parameters for the method parameters
	/// * <see cref="StatementSyntax"/> - zero or more statements for the method body
	/// </summary>
	public class MethodDeclarationBuilder : INodeBuilder {
		public MethodDeclarationBuilder(TypeSyntax returnType, string methodName) {
			Node = SyntaxFactory.MethodDeclaration(returnType, methodName);
			Public();
		}
		public MethodDeclarationBuilder(string returnType, string methodName) : this(SyntaxFactory.ParseTypeName(returnType), methodName) { }
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


		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var statements = new List<StatementSyntax>();
			foreach (var element in elements) {
				if (element is ParameterSyntax parameter) {
					Node = Node.AddParameterListParameters(parameter);
				} else {
					statements.Add(new StatementNode(element).StatementSyntax);
				}
			}
			Node = Node.WithBody(SyntaxFactory.Block(statements));
			return Node;
		}
	}
}