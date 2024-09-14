using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// The default constructor of the <see cref="IdentifierNode"/> creates a <see cref="ThisExpressionSyntax"/>
	/// </summary>
	public class IdentifierNode : NodeContainer {
		public IdentifierNode(string name) : base(SyntaxFactory.IdentifierName(name)) { }
		public IdentifierNameSyntax Identifier => (IdentifierNameSyntax)Node;
	}
}
