using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Albatross.Authentication.Core;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.Hosting {
	[Route("api/app-info")]
	[ApiController]
	public class AppInfoController : ControllerBase {
		private readonly ProgramSetting programSetting;
		private readonly IGetCurrentUser getCurrentUser;

		public AppInfoController(ProgramSetting programSetting, IGetCurrentUser getCurrentUser) {
			this.programSetting = programSetting;
			this.getCurrentUser = getCurrentUser;
		}

		[HttpGet]
		public ProgramSetting Get() => programSetting;

		

		[HttpGet("assembly")]
		public IEnumerable<string> GetAssemblies([FromQuery]string[] name) {
			return FindAssembly(name);
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

		[HttpGet("user-claim")]
		[Authorize]
		public string[] GetUserClaims() { 
			var items = (from item in HttpContext.User?.Claims ?? new System.Security.Claims.Claim[0]
						 select $"{item.Type}: {item.Value}").ToArray();
			return items;
		}
		[HttpGet("user")]
		[Authorize]
		public string GetCurrentUser() {
			return getCurrentUser.Get();
		}
	}
}
