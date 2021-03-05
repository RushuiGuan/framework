using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Database {
	/// <summary>
	/// The interface will return all <see cref="Albatross.Database.Column"/> for the specified <see cref="Albatross.Database.Table"/> object.
	/// </summary>
	public interface IListTableColumn {
		IEnumerable<Column> List(Table table);
	}
}
