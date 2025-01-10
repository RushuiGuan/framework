using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class TypeNode : NodeContainer {
		const string Var_Text = "var";
		public TypeNode(string name) : this(SyntaxFactory.ParseTypeName(string.IsNullOrEmpty(name) ? Var_Text : name)) { }
		protected TypeNode(TypeSyntax type) : base(type) { }

		public TypeSyntax Type => (TypeSyntax)this.Node;

		public TypeNode NullableReferenceType() {
			if (this.Type.ToFullString() == Var_Text) {
				return this;
			} else {
				this.Node = SyntaxFactory.NullableType(this.Type);
				return this;
			}
		}
	}
}