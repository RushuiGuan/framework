using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Albatross.Authentication.Core;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Albatross.Hosting {
	[Route("api/app-info")]
	[ApiController]
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

		List<string> FindAssembly(IEnumerable<string> names) {
			List<string> result = new List<string>();
			foreach (var name in names) {
				try {
					result.Add(Assembly.Load(name).FullName);
				} catch {
					result.Add($"{name}: not found");
				}
			}
			return result;
		}

		[HttpGet]
		public ProgramSetting Get() => programSetting;

		[HttpGet("env")]
		public EnvironmentSetting GetEnvironment() => environmentSetting;

		[HttpGet("assembly")]
		public IEnumerable<string> GetAssemblies([FromQuery] string[] name) => FindAssembly(name);

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