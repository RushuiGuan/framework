using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Database {
	/// <summary>
	/// The interface will return all the <see cref="Albatross.Database.Index"/> of the specified <see cref="Albatross.Database.Table"/>
	/// </summary>
	public interface IListTableIndex {
		IEnumerable<Index> List(Table table);
	}
}
