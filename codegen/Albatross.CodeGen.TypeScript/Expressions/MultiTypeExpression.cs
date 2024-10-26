using Albatross.CodeGen.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class MultiTypeExpression : ListOfSyntaxNodes<ITypeExpression>, ITypeExpression {
		public MultiTypeExpression(params ITypeExpression[] nodes) : base(nodes) { }
		public MultiTypeExpression(IEnumerable<ITypeExpression> nodes) : base(nodes) { }

		protected override string Separator => "|";
		public bool Optional { get; init; }
	}
}