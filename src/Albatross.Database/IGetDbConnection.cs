using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// Provided with a <see cref="Albatross.Database.Database"/> input, the interface will return a database connection object
	/// </summary>
    public interface IGetDbConnection
    {
		IDbConnection Get(Database database);
    }
}
