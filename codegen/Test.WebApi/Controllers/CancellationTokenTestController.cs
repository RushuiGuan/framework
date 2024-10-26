using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Test.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class CancellationTokenTestController : ControllerBase {
		private readonly ILogger logger;

		public CancellationTokenTestController(ILogger logger) {
			this.logger = logger;
		}

		[HttpGet()]
		public async Task<string> Get(CancellationToken cancellationToken) {
			await Task.Delay(10000, cancellationToken);
			return "Hello, World!";
		}
	}
}