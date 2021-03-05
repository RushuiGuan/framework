using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The interface will return all the <see cref="Albatross.Database.SqlType"/> in the specified <see cref="Albatross.Database.Database"/>
	/// </summary>
    public interface IListSqlType {
		IEnumerable<SqlType> List(Database database);
    }
}
