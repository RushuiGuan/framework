using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Albatross.Commands.Daemon {
	[Route("api/command-queue")]
	[ApiController]
	public class CommandQueueController : ControllerBase {
		private readonly ICommandBus commandBus;

		public CommandQueueController(ICommandBus commandBus) {
			this.commandBus = commandBus;
		}

		[HttpGet()]
		public IEnumerable<string> GetCommandQueues() {
			return this.commandBus.GetAllQueues();
		}

		[HttpGet("{name}")]
		public CommandQueueDto GetCommandQueue(string name) {
			return this.commandBus.Get(name).CreateDto();
		}

		[HttpPost("{name}")]
		public void Signal(string name) => this.commandBus.Get(name).Signal();
	}
}
