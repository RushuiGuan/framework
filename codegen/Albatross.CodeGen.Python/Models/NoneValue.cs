using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class NoneValue : ModuleCodeElement {
		public NoneValue() : base(string.Empty, string.Empty) {
		}
		public override TextWriter Generate(TextWriter writer) => writer.Append("None");
	}
}