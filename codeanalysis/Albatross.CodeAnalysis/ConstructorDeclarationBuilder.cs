using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis {
	/// <summary>
	/// Create a <see cref="ConstructorDeclarationSyntax"/>.  Expects:
	/// * <see cref="ParameterSyntax"/> - zero or more optional parameters for the constructor parameters
	/// * <see cref="ArgumentListSyntax"/> - argument list for the base constructor call
	/// </summary>
	public class ConstructorDeclarationBuilder : INodeBuilder {
		public ConstructorDeclarationBuilder(string className) {
			Node = SyntaxFactory.ConstructorDeclaration(className);
			Public();
		}
		public ConstructorDeclarationBuilder Public() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public ConstructorDeclarationBuilder Private() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
			return this;
		}
		public ConstructorDeclarationBuilder Protected() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
			return this;
		}
		public ConstructorDeclarationSyntax Node { get; private set; }


		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			this.Node = this.Node.AddParameterListParameters(elements.OfType<ParameterSyntax>().ToArray());
			var argumentList = elements.OfType<ArgumentListSyntax>().FirstOrDefault();
			if (argumentList != null) {
				this.Node = this.Node.WithInitializer(SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, argumentList));
			}
			var statements = elements.OfType<StatementSyntax>().ToArray();
			this.Node = this.Node.WithBody(SyntaxFactory.Block(statements));
			return this.Node;
		}
	}
}
