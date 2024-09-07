using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class TypeNode : NodeContainer {
		public TypeNode(string name)
			: base(SyntaxFactory.ParseTypeName(name)) { }

		public TypeSyntax Type => (TypeSyntax)this.Node;
	}
}
