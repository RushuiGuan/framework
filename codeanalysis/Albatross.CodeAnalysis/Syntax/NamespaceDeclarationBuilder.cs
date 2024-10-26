using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class NamespaceDeclarationBuilder : INodeBuilder {
		public NamespaceDeclarationBuilder(string name) {
			this.name = name;
		}
		public NamespaceDeclarationBuilder DisableNullable() {
			disableNullable = true;
			return this;
		}

		private bool disableNullable = false;
		private readonly string name;

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var node = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(name));
			var usingDirectives = new List<UsingDirectiveSyntax>();
			var memberDeclarations = new List<MemberDeclarationSyntax>();
			foreach (var element in elements) {
				if (element is UsingDirectiveSyntax usingDirectiveSyntax) {
					usingDirectives.Add(usingDirectiveSyntax);
				} else if (element is MemberDeclarationSyntax memberDeclarationSyntax) {
					memberDeclarations.Add(memberDeclarationSyntax);
				} else {
					throw new InvalidOperationException($"SyntaxNode of type {element.GetType().Name} cannot be added to namespace");
				}
			}

			var distinctUsingDirectives = usingDirectives
				.Select(x => x.Name?.ToFullString() ?? throw new InvalidOperationException("Using Directive with Aliases is not supported"))
				.Distinct().OrderBy(x => x).Select(x => (UsingDirectiveSyntax)new UsingDirectiveNode(x).Node).ToArray();

			node = node.AddUsings(distinctUsingDirectives)
				.AddMembers(memberDeclarations.ToArray());
			if (!this.disableNullable) {
				node = node.CreateNamespaceNullableDirective();
			}
			return node;
		}
	}
}