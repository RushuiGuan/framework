using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteClass : CodeGeneratorBase<Class> {
		private readonly WriteClassProperty writeProperty;
		private readonly WriteImport writeImport;
		private readonly WriteMethod writeMethod;

		public WriteClass(WriteClassProperty writeProperty, WriteImport writeImport, WriteMethod writeMethod) {
			this.writeProperty = writeProperty;
			this.writeImport = writeImport;
			this.writeMethod = writeMethod;
		}

		public override void Run(TextWriter writer, Class @class) {
			foreach(var import in @class.Imports) {
				writeImport.Run(writer, import);
			}

			if (@class.Export) {
				writer.Append("export ");
			}
			writer.Append("class ");
			using(var scope = writer.BeginScope(@class.Name)) {
					foreach (var property in @class.Properties) {
						writer.Run(writeProperty, property).WriteLine();
					}
				foreach (var method in @class.Methods) {
					writer.Run(writeMethod, method);
				}
			}
		}
	}
}
