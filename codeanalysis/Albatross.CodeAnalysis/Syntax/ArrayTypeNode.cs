using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Albatross.CodeAnalysis.Syntax {
	public class ArrayTypeNode : TypeNode {
		public ArrayTypeNode(string elementType, int? size = null) : this(new TypeNode(elementType), size) { }
		public ArrayTypeNode(TypeNode elementType, int? size = null) : base(CreateCollectionType(elementType, size)) { }
		public ArrayTypeSyntax ArrayType => (ArrayTypeSyntax)this.Node;

		static ArrayTypeSyntax CreateCollectionType(TypeNode elementType, int? size) {
			if (size < 0) { throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater or equal to zero"); ; }
			ExpressionSyntax sizeSyntaxNode;
			if (size == null) {
				sizeSyntaxNode = SyntaxFactory.OmittedArraySizeExpression();
			} else {
				sizeSyntaxNode = new LiteralNode(size.Value).LiteralExpression;
			}

			return SyntaxFactory.ArrayType(elementType.Type, SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
				SyntaxFactory.ArrayRankSpecifier(
					SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(sizeSyntaxNode)
				)
			));
		}
	}
}