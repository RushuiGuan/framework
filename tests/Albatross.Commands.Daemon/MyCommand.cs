using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	public class MyCommand : Command<int> {
		public bool Fail { get; init; }

		public MyCommand(bool fail, string id) : base(id) {
			this.Fail = fail;
		}
	}
}
