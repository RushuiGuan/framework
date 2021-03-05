using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// Provide with a <see cref="Albatross.Database.Database"/> object, the schema and name of a sql type, the interface will query the database and populate a <see cref="Albatross.Database.SqlType"/> object
	/// For system types, schema could be null or in case of microsoft sql server - "sys".
	/// </summary>
	public interface IGetSqlType
    {
		SqlType Get(Database database, string schema, string name);
    }
}
