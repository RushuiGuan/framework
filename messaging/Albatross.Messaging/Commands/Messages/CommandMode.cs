using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands.Messages {
	public enum CommandMode : short {
		Internal = 0,
		FireAndForget = 1,
		Callback = 2,
	}
}