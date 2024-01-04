using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Field : ICodeElement {
		public ICodeElement Value { get; set; }

		public bool Static { get; set; }
		public string Name { get; set; }
		public PythonType Type { get; set; }

		public Field(string name, PythonType type, ICodeElement value) {
			this.Name = name;
			Type = type;
			this.Value = value;
		}
		public TextWriter Generate(TextWriter writer) {
			if(!Static) {
				writer.Append(My.Keywords.Self).Append(".");
			}
			writer.Append(Name).Code(Type);
			writer.Append(" = ").Code(Value);
			return writer;
		}
	}
}