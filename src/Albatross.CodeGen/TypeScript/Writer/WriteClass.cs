using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteClass : CodeGeneratorBase<Class> {
		private readonly WriteTypeScriptProperty writeProperty;
		private readonly WriteImport writeImport;
		private readonly WriteMethod writeMethod;

		public WriteClass(WriteTypeScriptProperty writeProperty, WriteImport writeImport, WriteMethod writeMethod) {
			this.writeProperty = writeProperty;
			this.writeImport = writeImport;
			this.writeMethod = writeMethod;
		}

		public override void Run(TextWriter writer, Class item) {
			foreach(var import in item.Imports ?? new string[0]) {
				writeImport.Run(writer, import);
			}

			writer.Append("export ").Append("class ");
			using(var scope = writer.BeginScope(item.Name)) {
				foreach (var property in item.Properties) {
					scope.Writer.Run(writeProperty, property);
				}
				if (item.Constructor != null) {
					scope.Writer.Run(writeMethod, item.Constructor);
				}
				foreach (var method in item.Methods) {
					scope.Writer.Run(writeMethod, method);
				}
			}
			writer.WriteLine();
		}
	}
}
