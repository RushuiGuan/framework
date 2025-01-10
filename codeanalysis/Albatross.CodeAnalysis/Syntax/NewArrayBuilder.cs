using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create an <see cref="ArrayCreationExpressionSyntax"/>.  
	/// </summary>
	public class NewArrayBuilder : INodeBuilder {
		private readonly ArrayTypeSyntax typeSyntax;

		private NewArrayBuilder(ArrayTypeSyntax typeSyntax) {
			Node = SyntaxFactory.ArrayCreationExpression(typeSyntax);
			this.typeSyntax = typeSyntax;
		}
		public NewArrayBuilder(ArrayTypeNode typeNode) : this((ArrayTypeSyntax)typeNode.Type) { }
		public NewArrayBuilder(string typeName) : this(new ArrayTypeNode(new TypeNode(typeName))) { }
		public ArrayCreationExpressionSyntax Node { get; private set; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Any()) {
				Node = Node.WithInitializer(SyntaxFactory.InitializerExpression(
									SyntaxKind.ArrayInitializerExpression,
									SyntaxFactory.SeparatedList(elements.Cast<ExpressionSyntax>())));
			}
			return Node;
		}
	}

}