using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
    public interface IDeployProcedure {
		void Deploy(Procedure procedure);
    }
}
