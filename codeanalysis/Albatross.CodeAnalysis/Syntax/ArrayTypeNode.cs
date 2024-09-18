using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class ArrayTypeNode : TypeNode {
		public ArrayTypeNode(TypeNode elementType) : base(CreateCollectionType(elementType)) { }

		static ArrayTypeSyntax CreateCollectionType(TypeNode elementType) {
			return SyntaxFactory.ArrayType(elementType.Type, SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
				SyntaxFactory.ArrayRankSpecifier(
					SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
						SyntaxFactory.OmittedArraySizeExpression()
					)
				)
			));
		}
	}
}
