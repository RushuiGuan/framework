using Albatross.Messaging.Commands;
using Microsoft.AspNetCore.Mvc;
using Sample.Core.Commands.MyOwnNameSpace;
using System.Threading.Tasks;

namespace Sample.WebApi.Controllers {
	[Route("api/command")]
	[ApiController]
	public class CommandController : ControllerBase {
		private readonly ICommandClient commandClient;

		public CommandController(ICommandClient commandClient) {
			this.commandClient = commandClient;
		}

		[HttpPost("system")]
		public async Task SubmitSystemCommand(ISystemCommand systemCommand) {
			await this.commandClient.Submit(systemCommand);
		}

		[HttpPost("app")]
		public async Task SubmitAppCommand(IApplicationCommand applicationCommand) {
			await this.commandClient.Submit(applicationCommand);
		}
	}
}
