using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteTypeScriptFile : CodeGeneratorBase<TypeScriptFile> {
		private readonly ICodeGenerator<Import> writeImport;
		private readonly ICodeGenerator<Enum> writeEnum;
		private readonly ICodeGenerator<Interface> writeInterface;
		private readonly ICodeGenerator<Class> writeClass;

		public WriteTypeScriptFile(ICodeGenerator<Import> writeImport, ICodeGenerator<Enum> writeEnum, ICodeGenerator<Interface> writeInterface, ICodeGenerator<Class> writeClass) {
			this.writeImport = writeImport;
			this.writeEnum = writeEnum;
			this.writeInterface = writeInterface;
			this.writeClass = writeClass;
		}

		public override void Run(TextWriter writer, TypeScriptFile file) {
			foreach(var item in file.Imports) {
				if (item.Items.Count > 0) {
					writeImport.Run(writer, item);
				}
			}
			foreach(var item in file.Enums) {
				writeEnum.Run(writer, item);
			}
			foreach (var item in file.Interfaces) {
				writeInterface.Run(writer, item);
			}
			foreach(var item in file.Classes) {
				writeClass.Run(writer, item);
			}
		}
	}
}
