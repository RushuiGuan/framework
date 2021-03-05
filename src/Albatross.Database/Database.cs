using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Database {
	/// <summary>
	/// A particular database on a server.  The object should have enough information to generate a database connection string.
	/// </summary>
	public class Database {
		/// <summary>
		/// The database server host name or IP address
		/// </summary>
		public string DataSource { get; set; }

		/// <summary>
		/// The database name
		/// </summary>
		public string InitialCatalog { get; set; }

		/// <summary>
		/// If integrated security is used for the database connection
		/// </summary>
		public bool SSPI { get; set; }

		/// <summary>
		/// If SSPI is false, SQL authentication will require UserName and Password properties
		/// </summary>
		public string UserName { get; set; }
		public string Password { get; set; }

		/// <summary>
		/// If the ConnectionString property is specified, the implementation of <see cref="Albatross.Database.IGetConnectionString"/> might use it directly instead of generating a connection string from the other properties
		/// </summary>
		public string ConnectionString { get; set; }
	}
}
