using System.IO;

namespace Albatross.CodeGen.Core {
	public interface ICodeElement {
		TextWriter Generate(TextWriter writer);
	}
}
