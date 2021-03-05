using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// Provide with an <see cref="Albatross.Database.Database" /> object, the interface can produce a connection string by invoking the <see cref="Albatross.Database.IGetConnectionString.Get(Database)" /> function.
	/// </summary>
    public interface IGetConnectionString
    {
		string Get(Database database);
    }
}
