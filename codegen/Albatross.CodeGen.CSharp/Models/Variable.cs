using System.IO;

namespace Albatross.CodeGen.CSharp.Models {
	public class Variable : ICodeElement {
		public const char VerbatimCharacter = '@';
		public bool UseVerbatimCharacter { get; set; }
		public Variable(string name, bool useVerbatimCharacter) {
			this.Name = name;
			UseVerbatimCharacter = useVerbatimCharacter;
		}
		public string Name { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (UseVerbatimCharacter && !Name.StartsWith(VerbatimCharacter)) {
				writer.Write(VerbatimCharacter);
			}
			writer.Write(this.Name);
			return writer;
		}
	}
}