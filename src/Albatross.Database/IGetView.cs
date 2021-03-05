using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// Provided with a <see cref="Albatross.Database.Database"/> object, schema and name, the interface will query the database and return a <see cref="Albatross.Database.View"/> object.
	/// </summary>
    public interface IGetView {
		View Get(Database database, string schema, string name);
    }
}
