using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database {
	/// <summary>
	/// search the database for table that match the criteria
	/// </summary>
	public interface IListTable {
		IEnumerable<Table> Get(Database database, string criteria);
	}
}
