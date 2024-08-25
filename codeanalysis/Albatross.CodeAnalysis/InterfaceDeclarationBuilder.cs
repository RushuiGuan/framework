using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class InterfaceDeclarationBuilder : INodeBuilder {
		public InterfaceDeclarationBuilder(string interfaceName) {
			this.Node = SyntaxFactory.InterfaceDeclaration(interfaceName);
			Public();
		}
		public InterfaceDeclarationSyntax Node{get; private set; }
		public InterfaceDeclarationBuilder Public() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public InterfaceDeclarationBuilder Partial() {
			this.Node = this.Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
			return this;
		}
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var attributes = elements.OfType<AttributeSyntax>().ToArray();
			if (attributes.Any()) {
				this.Node = this.Node.WithAttributeLists(SyntaxFactory.List(attributes.Select(x => SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(x)))));
			}
			return this.Node;
		}
	}
}
