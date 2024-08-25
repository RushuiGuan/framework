using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class ClassDeclarationBuilder : INodeBuilder {
		public ClassDeclarationBuilder(string className) {
			this.Node = SyntaxFactory.ClassDeclaration(className);
			Public();
		}
		public ClassDeclarationSyntax Node{get; private set; }
		public ClassDeclarationBuilder Public() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var constructor = elements.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
			if (constructor != null) {
				this.Node = this.Node.AddMembers(constructor);
			}
			var attributes = elements.OfType<AttributeSyntax>().ToArray();
			if (attributes.Any()) {
				this.Node = this.Node.WithAttributeLists(SyntaxFactory.List(attributes.Select(x => SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(x)))));
			}
			return this.Node;
		}
	}
}
