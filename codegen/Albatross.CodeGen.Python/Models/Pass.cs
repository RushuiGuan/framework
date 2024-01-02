using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Pass : ICodeElement {
		public TextWriter Generate(TextWriter writer) => writer.Append("pass");
	}
}
