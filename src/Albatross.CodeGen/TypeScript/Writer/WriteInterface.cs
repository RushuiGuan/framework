using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Reflection;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteInterface : CodeGeneratorBase<Interface> {
		WriteTypeScriptProperty writeProperty;
		public WriteInterface(WriteTypeScriptProperty writeProperty) {
			this.writeProperty = writeProperty;
		}

		public override void Run(TextWriter writer, Interface t) {
			writer.Append("export ").Append("interface ");
			if (t.IsGeneric) {
				writer.Append(t.Name.GetGenericTypeName());
				writer.Append("<");
				writer.WriteItems(t.GenericTypes, ",");
				writer.Append(">");
			} else {
				writer.Append(t.Name);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var property in t.Properties) {
					scope.Writer.Run(writeProperty, property);
				}
			}
			writer.WriteLine();
		}
	}
}
