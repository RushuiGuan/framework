using System.Linq;
using Albatross.Authentication;
using Albatross.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Albatross.Hosting {
	[Route("api/app-info")]
	[ApiController]
	[Authorize]
	public class AppInfoController : ControllerBase {
		private readonly ProgramSetting programSetting;
		private readonly IGetCurrentUser getCurrentUser;
		private readonly EnvironmentSetting environmentSetting;
		private readonly ILogger logger;

		public AppInfoController(ProgramSetting programSetting, IGetCurrentUser getCurrentUser, EnvironmentSetting environmentSetting, ILogger logger) {
			this.programSetting = programSetting;
			this.getCurrentUser = getCurrentUser;
			this.environmentSetting = environmentSetting;
			this.logger = logger;
		}

		[HttpGet]
		public ProgramSetting Get() => programSetting;

		[HttpGet("env")]
		public EnvironmentSetting GetEnvironment() => environmentSetting;

		[HttpGet("user-claim")]
		[Authorize]
		public string[] GetUserClaims() => HttpContext.User?.Claims?.Select(args => $"{args.Type}: {args.Value}")?.ToArray() ?? new string[0];

		[Authorize]
		[HttpGet("user")]
		public string GetCurrentUser() => getCurrentUser.Get();

		[HttpPost("log")]
		public void Log([FromQuery] LogLevel level, [FromBody] string msg) => logger.Log(level, msg);
	}
}