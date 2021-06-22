﻿using Microsoft.AspNetCore.Mvc;
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
		public async Task<int[]> SendMyCommand(bool fail) {
			List<Task<int>> list = new List<Task<int>>();
			for(int i=0; i<100; i++) {
				MyCommand cmd = new MyCommand(fail, i.ToString());
				list.Add(commandBus.Submit(cmd));
			}
			int[] result = await Task.WhenAll(list);
			return result;
		}
	}
}