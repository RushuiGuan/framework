using System;
using System.Collections.Generic;
using Albatross.Database;

namespace Albatross.CodeGen.Sql.Model { 
	public class UpdateStatement {

		IEnumerable<Column> Columns { get; set; }

	}
}
