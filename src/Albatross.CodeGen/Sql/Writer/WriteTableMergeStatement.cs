using System.IO;
using Albatross.CodeGen.Core;
using Albatross.Database;

namespace Albatross.CodeGen.Sql.Writer {
	public class WriteTableMergeStatement : CodeGeneratorBase<Table> {
		public override void Run(TextWriter writer, Table source) {
			writer.Append("merge").Append(" [").Append(source.Schema).Append("].[").Append(source.Name).Append("] dst");
		}
	}
}
