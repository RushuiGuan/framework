using System;
using System.IO;
using Albatross.CodeGen.Core;

namespace Albatross.CodeGen.Sql.Writer {
	public static class Extension {
		public static TextWriter EscapeSql(this TextWriter writer, string content) {
			return writer.Append("[").Append(content).Append("]");
		}

		public static CodeGeneratorScope BeginSqlScope(this TextWriter writer) {
			return new CodeGeneratorScope(writer, args => args.WriteLine(), args => args.WriteLine());
		}
	}
}
