using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class NoneValue : ICodeElement {
		public TextWriter Generate(TextWriter writer) => writer.Append("None");
	}
}
