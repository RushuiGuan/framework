using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.CSharp.Models {
	public class Constructor : Method {
		public Constructor(string name) : base(name) { }

		public MethodCall? BaseConstructor { get; set; }

		public override TextWriter Generate(TextWriter writer) {
			if (Static) {
				writer.Static();
			} else {
				writer.Code(new AccessModifierElement(AccessModifier)).Space();
			}
			writer.Append(Name).OpenParenthesis();
			writer.Code(new ParameterCollection(Parameters));
			writer.CloseParenthesis();

			if (BaseConstructor != null) {
				writer.Append(" : ");
				BaseConstructor.Generate(writer);
			}
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(this.CodeBlock);
			}
			return writer;
		}
	}
}