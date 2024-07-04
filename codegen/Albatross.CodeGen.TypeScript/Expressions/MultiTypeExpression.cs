using Albatross.CodeGen.Syntax;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class MultiTypeExpression : ListOfSyntaxNodes<ITypeExpression>, ITypeExpression {
		protected override string Separator => "|";
		public bool Optional { get; init; }
	}
}
