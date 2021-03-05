using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
    public interface IGetProcedureDefinition
    {
		string Get(Procedure procedure);
    }
}
