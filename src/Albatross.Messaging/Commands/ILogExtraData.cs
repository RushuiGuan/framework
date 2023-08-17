using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public interface ILogExtraData {
		object Target => this;
	}
}
