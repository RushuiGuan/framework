using Microsoft.AspNetCore.Mvc;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using System.Threading.Tasks;

namespace Sample.WebApi.Controllers {
	[Route("api/command")]
	[ApiController]
	public class CommandController : ControllerBase {
		private readonly IMyCommandClient commandClient;

		public CommandController(IMyCommandClient commandClient) {
			this.commandClient = commandClient;
		}

		[HttpPost("system")]
		public async Task<ulong> SubmitSystemCommand([FromBody] ISystemCommand systemCommand) {
			if (systemCommand.Callback) {
				return await this.commandClient.SubmitWithCallback(systemCommand);
			} else {
				return await this.commandClient.Submit(systemCommand);
			}
		}

		[HttpPost("app")]
		public async Task<ulong> SubmitAppCommand([FromBody] IApplicationCommand applicationCommand) {
			return await this.commandClient.SubmitWithCallback(applicationCommand);
		}


		[HttpPost("command-serialization-error-test")]
		public async Task<ulong> CommandSerializationErrorTest(bool callback) {
			if (callback) {
				return await this.commandClient.SubmitWithCallback(new SerializationErrorTestCommand("a", "b"));
			} else {
				return await this.commandClient.Submit(new SerializationErrorTestCommand("a", "b"));
			}
		}
	}
}