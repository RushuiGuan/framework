using System;
using System.Collections.Generic;
using Albatross.Database;

namespace Albatross.CodeGen.Sql.Model {
	public class DeclareStatement {
		public IEnumerable<Variable> Variables { get; set; }
	}
}
