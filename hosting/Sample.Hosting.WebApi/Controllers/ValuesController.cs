using Microsoft.AspNetCore.Mvc;
using System;

namespace Sample.Hosting.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase {
		[HttpGet]
		public string Get() {
			return DateTime.UtcNow.ToString();
		}
	}
}
