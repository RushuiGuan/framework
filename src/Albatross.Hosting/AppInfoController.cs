using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.Hosting {
	[Route("api/app-info")]
	[ApiController]
	public class AppInfoController : ControllerBase {
		private readonly ProgramSetting programSetting;

		public AppInfoController(ProgramSetting programSetting) {
			this.programSetting = programSetting;
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
	}
}
