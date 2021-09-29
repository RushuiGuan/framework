using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteEnum : CodeGeneratorBase<Enum> {
		public override void Run(TextWriter writer, Enum @enum) {
			writer.Append("export ");
			writer.Append("enum ");
			using(var scope = writer.BeginScope(@enum.Name)) {
				foreach(var value in @enum.Values) {
					scope.Writer.Append(value).WriteLine(",");
				}
			}
			writer.WriteLine();
		}
	}
}
