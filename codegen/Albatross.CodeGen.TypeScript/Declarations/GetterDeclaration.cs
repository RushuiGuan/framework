using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class GetterDeclaration : MethodDeclaration, ICodeElement {
		public GetterDeclaration(string name) : base(name) {
			Parameters = new Syntax.ListOfSyntaxNodes<ParameterDeclaration>();
		}

		public override TextWriter Generate(TextWriter writer) {
			var modifier = Modifiers.Where(x => x is AccessModifier).FirstOrDefault() ?? AccessModifier.Public;
			if (!object.Equals(modifier, AccessModifier.Public)) {
				writer.Append(modifier.Name).Space();
			}
			writer.Append("get ");
			writer.Code(Identifier).OpenParenthesis().Code(Parameters).CloseParenthesis();
			if (!object.Equals(this.ReturnType, Defined.Types.Void())) {
				writer.Append(": ").Code(ReturnType).Space();
			}
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(Body);
			}
			writer.WriteLine();
			return writer;
		}
	}
}