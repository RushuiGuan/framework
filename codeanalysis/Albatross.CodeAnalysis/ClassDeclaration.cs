using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class ClassDeclaration : INodeBuilder {
		public ClassDeclaration(string className) {
			this.Node = SyntaxFactory.ClassDeclaration(className);
			Public();
		}
		public ClassDeclarationSyntax Node{get; private set; }
		public ClassDeclaration Public() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var constructor = elements.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
			if (constructor != null) {
				this.Node = this.Node.AddMembers(constructor);
			}
			return this.Node;
		}
	}
}
