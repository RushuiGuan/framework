using Microsoft.AspNetCore.Mvc;
using System;

namespace Test.WebApi.Controllers {
	[Route("api/redirect-test")]
	[ApiController]
	public class RedirectTestController : ControllerBase {

		[HttpGet("test-0")]
		public IActionResult Get() {
			return RedirectToAction(nameof(Get1));
		}

		[HttpGet("test-1")]
		public IActionResult Get1() {
			return RedirectToAction(nameof(Get2));
		}

		[HttpGet("test-2")]
		public IActionResult Get2() {
			return RedirectToAction(nameof(Get3));
		}

		[HttpGet("test-3")]
		public IActionResult Get3() {
			return RedirectToAction(nameof(Get4));
		}

		[HttpGet("test-4")]
		public IActionResult Get4() {
			return RedirectToAction(nameof(Get5));
		}

		[HttpGet("test-5")]
		public IActionResult Get5() {
			return RedirectToAction(nameof(Get6));
		}

		[HttpGet("test-6")]
		public IActionResult Get6() {
			return RedirectToAction(nameof(Get7));
		}

		[HttpGet("test-7")]
		public IActionResult Get7() {
			return RedirectToAction(nameof(Get8));
		}

		[HttpGet("test-8")]
		public IActionResult Get8() {
			return RedirectToAction(nameof(Get9));
		}

		[HttpGet("test-9")]
		public IActionResult Get9() {
			return RedirectToAction(nameof(Get10));
		}

		[HttpGet("test-10")]
		public string Get10() {
			return "Here is the end of the redirect chain";
		}
	}
}