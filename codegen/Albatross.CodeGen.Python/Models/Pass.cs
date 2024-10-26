using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Pass : ModuleCodeElement {
		public Pass() : base(string.Empty, string.Empty) { }
		public override TextWriter Generate(TextWriter writer) => writer.AppendLine().Append("pass");
	}
}