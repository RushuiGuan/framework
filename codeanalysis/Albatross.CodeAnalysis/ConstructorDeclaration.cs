using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis {
	/// <summary>
	/// Create a <see cref="ConstructorDeclarationSyntax"/>
	/// Expects an optional <see cref="ParameterListSyntax"/>.  Expects an option <see cref="ArgumentListSyntax"/>.  If found, will initialize the constructor
	/// with the base constructor call using the argument list.
	/// </summary>
	public class ConstructorDeclaration : INodeBuilder {
		public ConstructorDeclaration(string className) {
			Node = SyntaxFactory.ConstructorDeclaration(className);
			Public();
		}
		public ConstructorDeclaration Public() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public ConstructorDeclaration Private() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
			return this;
		}
		public ConstructorDeclaration Protected() {
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
			var statements = elements.OfType< StatementSyntax>().ToList();
			this.Node = this.Node.WithBody(SyntaxFactory.Block(statements));
			return this.Node;
		}
	}
}
