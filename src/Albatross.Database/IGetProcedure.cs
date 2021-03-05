using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// Provide with a <see cref="Albatross.Database.Database"/> object, the schema and name of a stored procedure, the interface will query the database and populate a <see cref="Albatross.Database.Procedure"/> object
	/// </summary>
    public interface IGetProcedure
    {
		Procedure Get(Database database, string schema, string name);
    }
}
