using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Albatross.CodeAnalysis.Syntax {
	public class ArrayTypeNode : TypeNode {
		public ArrayTypeNode(TypeNode elementType) : base(CreateCollectionType(elementType, null)) { }

		public ArrayTypeNode(TypeNode elementType, int? size) : base(CreateCollectionType(elementType, size)) { }

		static ArrayTypeSyntax CreateCollectionType(TypeNode elementType, int? size) {
			if (size < 0) {
				throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater or equal to zero"); ;
			}
			return SyntaxFactory.ArrayType(elementType.Type, SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
				SyntaxFactory.ArrayRankSpecifier(
					SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
						size.HasValue ?
						SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(size.Value)) :
						SyntaxFactory.OmittedArraySizeExpression()
					)
				)
			));
		}
	}
}