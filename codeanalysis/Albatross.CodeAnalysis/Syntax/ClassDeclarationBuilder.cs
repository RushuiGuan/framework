﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {

	/// <summary>
	/// Create a <see cref="ClassDeclarationSyntax"/> instance.  Will look for the following components: 
	/// * <see cref="ConstructorDeclarationSyntax"/>
	/// * <see cref="AttributeSyntax"/>"/>
	/// * <see cref="BaseTypeSyntax"/>
	/// </summary>
	public class ClassDeclarationBuilder : INodeBuilder {
		public ClassDeclarationBuilder(string className) {
			Node = SyntaxFactory.ClassDeclaration(className);
			Public();
		}
		public ClassDeclarationSyntax Node { get; private set; }
		public ClassDeclarationBuilder Public() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public ClassDeclarationBuilder Partial() {
			Node = Node.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
			return this;
		}
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var constructor = elements.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
			if (constructor != null) {
				Node = Node.AddMembers(constructor);
			}
			var attributes = elements.OfType<AttributeSyntax>().ToArray();
			if (attributes.Any()) {
				Node = Node.WithAttributeLists(SyntaxFactory.List(attributes.Select(x => SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(x)))));
			}
			var baseTypes = elements.OfType<BaseTypeSyntax>().ToArray();
			if (baseTypes.Any()) {
				Node = Node.AddBaseListTypes(baseTypes);
			}
			return Node;
		}
	}
}
