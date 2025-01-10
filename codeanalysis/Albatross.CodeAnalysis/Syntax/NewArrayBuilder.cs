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
		private readonly TypeNode elementTypeNode;
		public SyntaxNode Node { get; private set; }

		public NewArrayBuilder(string elementTypeName) : this(new TypeNode(elementTypeName)) { }
		public NewArrayBuilder(TypeNode elementTypeNode) {
			this.elementTypeNode = elementTypeNode;
			this.Node = SyntaxFactory.ArrayCreationExpression(new ArrayTypeNode(elementTypeNode, 0).ArrayType);
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Any()) {
				var arrayTypeNode = new ArrayTypeNode(elementTypeNode);
				this.Node = SyntaxFactory.ArrayCreationExpression(arrayTypeNode.ArrayType)
					.WithInitializer(SyntaxFactory.InitializerExpression(
									SyntaxKind.ArrayInitializerExpression,
									SyntaxFactory.SeparatedList(elements.Cast<ExpressionSyntax>())));
			}
			return Node;
		}
	}
}