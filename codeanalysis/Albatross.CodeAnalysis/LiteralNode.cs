using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis {
	public class LiteralNode : NodeContainer {
		public LiteralNode(string text)
			: base(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(text))) {
		}
		public LiteralNode(int intValue)
			: base(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(intValue))) {
		}
	}
	public class TypeNode : NodeContainer {
		public TypeNode(string name)
			: base(SyntaxFactory.ParseTypeName(name)) { }
	}
	public class IdentifierNode : NodeContainer {
		public IdentifierNode(string name)
			: base(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(name))) { }
	}
}
