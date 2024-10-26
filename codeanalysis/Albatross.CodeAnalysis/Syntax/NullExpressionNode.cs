using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	public class NullExpressionNode : NodeContainer {
		public NullExpressionNode() : base(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)) {
		}
	}
}