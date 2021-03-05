using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database {
	/// <summary>
	/// search the database for stored procedures that match the criteria
	/// </summary>
	public interface IListProcedure {
		IEnumerable<Procedure> Get(Database database, string criteria);
	}
}
