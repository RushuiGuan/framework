using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class IdentifierNode : NodeContainer {
		public IdentifierNode(string name) : base(SyntaxFactory.IdentifierName(name)) { }
		public IdentifierNameSyntax Identifier => (IdentifierNameSyntax)Node;
	}
}