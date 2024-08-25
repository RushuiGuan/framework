using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class NamespaceDeclarationBuilder : INodeBuilder {
		public NamespaceDeclarationBuilder(string name) {
			this.Node = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(name));
		}

		public NamespaceDeclarationSyntax Node { get; private set; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var members = new List<MemberDeclarationSyntax>();
			members.AddRange(elements.OfType<InterfaceDeclarationSyntax>());
			members.AddRange(elements.OfType<ClassDeclarationSyntax>());
			this.Node = this.Node.AddMembers(members.ToArray());

			var distinctUsingDirectives = elements.OfType<UsingDirectiveSyntax>()
				.Select(x => x.Name?.ToFullString() ?? throw new InvalidOperationException("Using Directive with Aliases is not supported"))
				.Distinct().OrderBy(x => x).Select(x => (UsingDirectiveSyntax)new UsingDirectiveNode(x).Node).ToArray();

			this.Node = this.Node.AddUsings(distinctUsingDirectives);
			return this.Node;
		}
	}
}
