using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.Logging.WebApi.Controllers {
	[Route("api/log")]
	[ApiController]
	public class LogController : ControllerBase {
		private readonly ILogger<LogController> logger;

		public LogController(ILogger<LogController> logger) {
			this.logger = logger;
		}

		[HttpPost("{level}")]
		public void Send([FromRoute] LogLevel level, [FromQuery] string message) {
			logger.Log(level, message);
		}
	}
}
