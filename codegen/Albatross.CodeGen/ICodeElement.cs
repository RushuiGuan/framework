using System.IO;

namespace Albatross.CodeGen {
	public interface ICodeElement {
		TextWriter Generate(TextWriter writer);
	}
}