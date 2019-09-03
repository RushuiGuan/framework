using System;
using System.IO;
using Albatross.CodeGen.Core;
using Albatross.CodeGen.Sql.Model;

namespace Albatross.CodeGen.Sql.Writer {
	public class WriteExpression : CodeGeneratorBase<Expression> {
		public override void Run(TextWriter writer, Expression source) {
			writer.Append(source.Value).Space().EscapeSql(source.Alias);
		}
	}
}
