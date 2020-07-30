using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Http;
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
		public ProgramSetting Get() => this.programSetting;
	}
}
