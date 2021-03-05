using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Database {
	/// <summary>
	/// The interface will return all the <see cref="Albatross.Database.IndexColumn"/> for the specified <see cref="Albatross.Database.Index"/>
	/// </summary>
	public interface IListTableIndexColumn {
		IEnumerable<IndexColumn> List(Index index);
	}
}
