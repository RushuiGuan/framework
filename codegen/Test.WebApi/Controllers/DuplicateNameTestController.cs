using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Test.WebApi.Controllers {
	[Route("api/duplicate-name-test")]
	[ApiController]
	public class CommandCDuplicateNameTestControllerontroller : ControllerBase {
		[HttpPost("by-id")]
		public Task Submit(int id) => Task.CompletedTask;

		[HttpPost("by-name")]
		public Task Submit(string name) => Task.CompletedTask;
	}
}