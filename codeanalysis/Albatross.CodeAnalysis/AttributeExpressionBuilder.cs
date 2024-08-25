using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis {
	/// <summary>
	/// Create a node of type <see cref="AttributeSyntax"/>.  Expect an optional parameter of type <see cref="AttributeArgumentListSyntax"/>.  
	/// </summary>
	public class AttributeBuilder : INodeBuilder {
		public AttributeBuilder(string name) {
			this.Node = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(name));
		}

		public AttributeSyntax Node { get; private set; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var argumentList = elements.OfType<AttributeArgumentListSyntax>().FirstOrDefault();
			if (argumentList != null) {
				this.Node = this.Node.WithArgumentList(argumentList);
			}
			return this.Node;
		}
	}
}
