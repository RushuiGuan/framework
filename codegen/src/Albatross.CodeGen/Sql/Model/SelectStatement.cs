using System;
using System.Collections.Generic;
using Albatross.Database;

namespace Albatross.CodeGen.Sql.Model {
	public class SelectStatement {
		SelectClause Select { get; set; }
		WhereClause Where { get; set; }
		GroupByClause GroupBy { get; set; }
		HavingClause Having { get; set; }
		IEnumerable<JoinClause> Joins { get; set; }
	}
}
