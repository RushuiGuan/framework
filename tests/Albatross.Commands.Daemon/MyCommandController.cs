using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	[Route("api/my-command")]
	[ApiController]
	public class MyCommandController : ControllerBase {
		private readonly ICommandBus commandBus;

		public MyCommandController(ICommandBus commandBus) {
			this.commandBus = commandBus;
		}

		[HttpPost]
		public void SendMyCommand(bool wait, bool fail) {
			MyCommand cmd = new MyCommand(wait, fail);
			commandBus.Submit(cmd);
			if (cmd.Exception != null) { throw cmd.Exception; }
		}
	}
}
