using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The interface will return the <see cref="Albatross.Database.SqlType"/> for the specified <see cref="Albatross.Database.Table" /> and <see cref="Albatross.Database.Column"/>
	/// </summary>
    public interface IGetTableColumnType
    {
		SqlType Get(Table table, string column);
	}
}
