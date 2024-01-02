using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.CSharp.Models {
	public class Parameter : ICodeElement {
		public Parameter(string name, DotNetType type) {
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }
		public DotNetType Type { get; set; }
		public ParameterModifier Modifier { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (Modifier == ParameterModifier.Out) {
				writer.Append("out ");
			} else if (Modifier == ParameterModifier.Ref) {
				writer.Append("ref ");
			} else if (Modifier == ParameterModifier.In) {
				writer.Append("in ");
			}
			writer.Code(Type);
			writer.Space().Append("@").Append(Name);
			return writer;
		}
	}
}