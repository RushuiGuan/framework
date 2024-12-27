using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class InterfaceDeclarationBuilder : INodeBuilder {
		public InterfaceDeclarationBuilder(string interfaceName) {
			Node = SyntaxFactory.InterfaceDeclaration(interfaceName);
		}
		public InterfaceDeclarationSyntax Node { get; private set; }
		public InterfaceDeclarationBuilder Public() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public InterfaceDeclarationBuilder Internal() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
			return this;
		}
		public InterfaceDeclarationBuilder Partial() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
			return this;
		}
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var attributes = elements.OfType<AttributeSyntax>().ToArray();
			if (attributes.Any()) {
				Node = Node.WithAttributeLists(SyntaxFactory.List(attributes.Select(x => SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(x)))));
			}
			var properties = elements.OfType<PropertyDeclarationSyntax>().ToArray();
			Node = Node.AddMembers(properties);
			var methods = elements.OfType<MethodDeclarationSyntax>().ToArray();
			Node = Node.AddMembers(methods);
			return Node;
		}
	}
}