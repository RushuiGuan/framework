using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
    public interface IGetDatabasePermission{
		IEnumerable<DatabasePermission> Get(IDatabaseObject @object);
    }
}
