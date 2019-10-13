using System;
using System.IO;
using Albatross.CodeGen.Core;
using Albatross.CodeGen.Sql.Model;

namespace Albatross.CodeGen.Sql.Writer {
	public class WriteSelectClause : CodeGeneratorBase<SelectClause> {
		ICodeGenerator<Expression> writeExpression;

		public WriteSelectClause(ICodeGenerator<Expression> writeExpression) {
			this.writeExpression = writeExpression;
		}

		public override void Run(TextWriter writer, SelectClause source) {
			writer.WriteLine("select");
			using (var scope = writer.BeginSqlScope()) {
				foreach (var item in source.Columns) {
					writer.Run(writeExpression, item).WriteLine();
				}
			}
		}
	}
}
