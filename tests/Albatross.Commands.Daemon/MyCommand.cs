using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	public class MyCommand : Command {
		public bool Fail { get; init; }

		public MyCommand(bool waitForCompletion, bool fail) : base(waitForCompletion) {
			this.Fail = fail;
		}
	}
}
