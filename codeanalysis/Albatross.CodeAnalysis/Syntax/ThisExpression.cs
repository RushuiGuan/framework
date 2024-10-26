using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	public class ThisExpression : NodeContainer {
		public ThisExpression() : base(SyntaxFactory.ThisExpression()) { }
	}
}