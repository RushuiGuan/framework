using Albatross.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Sample.Hosting.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase {
		private readonly ILogger logger;

		public ValuesController(ILogger logger) {
			this.logger = logger;
		}

		[HttpGet]
		public string Get() {
			return DateTime.UtcNow.ToString();
		}


		[HttpGet("error")]
		public ActionResult<string> TestError(string msg) {
			try {
				// return DateTime.UtcNow.ToString();
				throw new ArgumentException(msg);
			} catch (ArgumentException err) {
				return BadRequest(err.ErrorMessage(logger));
			}
		}

		[HttpGet("error2")]
		public ActionResult<string> TestError2(string msg) {
			try {
				// return DateTime.UtcNow.ToString();
				throw new ArgumentException(msg);
			} catch (ArgumentException err) {
				return BadRequest(err.ErrorTextMessage(logger));
			}
		}
	}
}
