using Albatross.CodeGen.Core;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Constructor : Method {
		public Constructor(string name) : base(name) { }

		public Constructor? BaseConstructor { get; set; }

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
				writer.Append(" : ").Append(BaseConstructor.Name).OpenParenthesis();
				writer.WriteItems(Parameters, ", ", (w, item) => { w.Append("@").Append(item.Name); });
				writer.CloseParenthesis();
			}
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(this.CodeBlock);
			}
			return writer;
		}
	}
}
