using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteImport : CodeGeneratorBase<Import> {
		public WriteImport() {
		}

		public override void Run(TextWriter writer, Import import) {
			// import {format, parse} from 'date-fns';
			writer.Append("import ");
			using(var scope = writer.BeginScope()) {
				scope.Writer.WriteItems(import.Items, ", ");
			}
			writer.Append(" from ").StringLiteral(import.From).Semicolon();
		}
	}
}
