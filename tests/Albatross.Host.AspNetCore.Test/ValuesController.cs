using System;
using System.Collections.Generic;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Albatross.Host.AspNetCore.Test {
	[Route("api/[controller]")]
	public class ValuesController : Controller {
		private readonly ProgramSetting setting;
		private readonly IConfiguration configuration;

		public ValuesController(ProgramSetting setting, IConfiguration configuration) {
			this.setting = setting;
			this.configuration = configuration;
		}

		[HttpGet]
		public string Get() {
			return setting.App;
		}

		[HttpGet("test-error")]
		public IEnumerable<string> TestException() {
			throw new Exception("test");
		}

		[HttpGet("settings")]
		public string GetSettings() {
			var setting = this.configuration.GetSection(TestSetting.Key).Get<TestSetting>();
			return $"{setting.Name} - {setting.Count}";
		}
	}
}
