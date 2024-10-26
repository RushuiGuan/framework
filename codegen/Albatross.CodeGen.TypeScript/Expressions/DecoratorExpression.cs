using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class DecoratorExpression : InvocationExpression {
		public override TextWriter Generate(TextWriter writer) {
			writer.Append('@');
			return base.Generate(writer);
		}
	}
}