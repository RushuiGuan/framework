using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.ReqRep {
	public interface IWorkerService : IDisposable {
		void Start();
	}
}