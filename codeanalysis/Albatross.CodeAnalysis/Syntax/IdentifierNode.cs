using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// The default constructor of the <see cref="IdentifierNode"/> creates a <see cref="ThisExpressionSyntax"/>
	/// </summary>
	public class IdentifierNode : NodeContainer {
		public IdentifierNode() : base(SyntaxFactory.ThisExpression()) { }
		public IdentifierNode(string name) : base(SyntaxFactory.IdentifierName(name)) { }

		public IdentifierNode WithMember(string name) {
			return new IdentifierNode() {
				Node = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, this.Identifier, SyntaxFactory.IdentifierName(name))
			};
		}

		public IdentifierNode WithGenericMember(string name, params string[] genericTypes) {
			return new IdentifierNode() {
				Node = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, this.Identifier, new GenericIdentifierNode(name, genericTypes).Identifier)
			};
		}
		public ExpressionSyntax Identifier => (ExpressionSyntax)Node;
	}
}
