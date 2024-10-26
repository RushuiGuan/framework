using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class TypeNode : NodeContainer {
		public TypeNode(string name) : this(SyntaxFactory.ParseTypeName(name)) { }
		protected TypeNode(TypeSyntax type) : base(type) { }

		public TypeSyntax Type => (TypeSyntax)this.Node;

		public TypeNode NullableReferenceType() {
			this.Node = SyntaxFactory.NullableType(this.Type);
			return this;
		}
	}
}