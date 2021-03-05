using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The class represents a database table
	/// </summary>
    public class Table : IDatabaseObject{
		public Database Database { get; set; }
		public string Name { get; set; }
		public string Schema { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public IEnumerable<Column> Columns { get; set; }
		public IEnumerable<IndexColumn> PrimaryKeys { get; set; }
		public Column IdentityColumn { get; set; }
		public IEnumerable<DatabasePermission> Permissions { get; set; }
	}
}
