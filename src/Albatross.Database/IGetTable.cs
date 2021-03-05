using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// Provided with a <see cref="Albatross.Database.Database"/> object, schema and name, the interface will query and return a <see cref="Albatross.Database.Table"/> object.
	/// </summary>
    public interface IGetTable
    {
		Table Get(Database database, string schema, string name);
    }
}
