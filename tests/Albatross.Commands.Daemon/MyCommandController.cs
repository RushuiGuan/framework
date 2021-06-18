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
		public async Task<int> SendMyCommand(bool fail) {
			MyCommand cmd = new MyCommand(fail, Guid.NewGuid().ToString());
			commandBus.Submit(cmd);
			int result = await cmd.Task;
			return result;
		}
	}
}
