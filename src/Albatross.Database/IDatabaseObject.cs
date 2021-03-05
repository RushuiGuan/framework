using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
    public interface IDatabaseObject
    {
		string Name { get; }
		string Schema { get; }
		Database Database { get; }
		IEnumerable<DatabasePermission> Permissions { get; set; }
	}
}
