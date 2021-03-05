using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The class represents a database stored procedure
	/// </summary>
	public class Procedure : IDatabaseObject { 
		public Database Database { get; set; }

		public string Name { get; set; }
		public string Schema{ get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public IEnumerable<Parameter> Parameters { get; set; }

		public string AlterScript { get; set; }
		public string CreateScript { get; set; }
		public string PermissionScript { get; set; }
		public IEnumerable<DatabasePermission> Permissions { get; set; }
	}
}
