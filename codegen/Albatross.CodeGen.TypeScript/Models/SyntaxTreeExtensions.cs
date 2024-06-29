namespace Albatross.CodeGen.TypeScript.Models {
	public static class SyntaxTreeExtensions {
		public static TypeExpression AnyType(this SyntaxTree syntaxTree) {
			return syntaxTree.Type("any");
		}
	}
}
