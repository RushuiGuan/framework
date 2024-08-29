using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis {
	public class TypeNode : NodeContainer {
		public TypeNode(string name)
			: base(SyntaxFactory.ParseTypeName(name)) { }
	}
}
