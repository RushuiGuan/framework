using System;
using System.Collections.Generic;
using Albatross.Database;

namespace Albatross.CodeGen.Sql.Model {
	public class SelectClause {
		public IEnumerable<Expression> Columns { get; set; }
	}
}
