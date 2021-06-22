using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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

		[HttpGet("active")]
		public IEnumerable<CommandQueueDto> GetActiveCommandQueues() {
			var names = this.commandBus.GetAllQueues();
			List<CommandQueueDto> list = new List<CommandQueueDto>();
			foreach(var name in names) {
				var queue = commandBus.Get(name).CreateDto();
				if(queue.Items.Count() > 0) {
					list.Add(queue);
				}
			}
			return list;
		}

		[HttpGet("{name}")]
		public CommandQueueDto GetCommandQueue(string name) {
			return this.commandBus.Get(name).CreateDto();
		}

		[HttpPost("{name}")]
		public void Signal(string name) => this.commandBus.Get(name).Signal();

	}
}
