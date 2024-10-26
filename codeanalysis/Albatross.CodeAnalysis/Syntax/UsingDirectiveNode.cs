using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	public class UsingDirectiveNode : NodeContainer {
		public UsingDirectiveNode(string name)
			: base(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(name))) {
		}
	}
}