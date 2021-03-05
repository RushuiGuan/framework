using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The class represents a table index
	/// </summary>
    public class Index{
		public Table Table { get; set; }
		public string Name { get; set; }
		public int Type { get; set; }
		public string TypeDesc { get; set; }
		public bool IsDisabled { get; set; }
		public bool IsUnique { get; set; }
		public bool IsUniqueConstraint { get; set; }
		public bool IsPrimaryKey { get; set; }

		public IEnumerable<IndexColumn> Columns { get; set; }
	}
}
