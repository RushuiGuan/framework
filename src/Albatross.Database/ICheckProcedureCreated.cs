using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
    public interface ICheckProcedureCreated
    {
		bool Check(Procedure procedure);
    }
}
