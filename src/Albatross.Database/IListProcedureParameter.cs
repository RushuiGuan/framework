using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The interface will query the database and return the list of <see cref="Albatross.Database.Parameter"/> for the <see cref="Albatross.Database.Procedure"/> object.
	/// </summary>
    public interface IListProcedureParameter
    {
		IEnumerable<Parameter> List(Procedure procedure);
    }
}
