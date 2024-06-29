using System.IO;

namespace Albatross.CodeGen {
	public class ModuleCodeElement : IModuleCodeElement {
		internal ModuleCodeElement(ICodeModule module, ICodeElement codeElement) {
			Module = module;
			CodeElement = codeElement;
		}
		public ICodeModule Module { get; }
		public ICodeElement CodeElement { get; }
		public TextWriter Generate(TextWriter writer) => CodeElement.Generate(writer);
	}
}
