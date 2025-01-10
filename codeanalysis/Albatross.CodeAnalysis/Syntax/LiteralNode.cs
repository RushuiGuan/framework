using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class LiteralNode : NodeContainer {
		public LiteralNode(string? text) : base(GetStringOrNullLiteral(text)) { }
		static LiteralExpressionSyntax GetStringOrNullLiteral(string? text) {
			if (text == null) {
				return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
			} else {
				return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(text));
			}
		}
		public LiteralNode(int intValue)
			: base(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(intValue))) {
		}
		public LiteralNode(bool boolValue)
			: base(boolValue ? SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression) : SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression)) { }
		
		public LiteralExpressionSyntax LiteralExpression => (LiteralExpressionSyntax)Node;
	}
}