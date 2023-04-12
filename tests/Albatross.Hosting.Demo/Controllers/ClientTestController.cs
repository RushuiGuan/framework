using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Albatross.Hosting.Demo.Controllers {
	public class MyResult {
		public MyResult(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
	}
	[Route("api/client-test")]
	[ApiController]
	public class ClientTestController : ControllerBase {
		[HttpGet("string-response")]
		public string StringResponse([FromQuery] string name) {
			return name;
		}

		[HttpGet("text-response")]
		public async Task TextResponse([FromQuery] string name) {
			await this.Response.WriteAsync(name);
		}

		[HttpGet("json-response")]
		public MyResult JsonResponse([FromQuery] string name) {
			return new MyResult(name);
		}

		[HttpGet("standard-error")]
		public MyResult StandardError([FromQuery] string name) {
			throw new InvalidOperationException("this call is not valid");
		}

		[HttpGet("custom-error")]
		public async Task CustomError([FromQuery] string name) {
			Response.StatusCode = 500;
			Response.ContentType = "application/json";
			await Response.WriteAsJsonAsync<MyResult>(new MyResult(name));
		}

		[HttpGet("no-response-1")]
		public string? NoContent1([FromQuery] string name) {
			return null;
		}

		[HttpGet("no-response-2")]
		public void NoContent2([FromQuery] string name) {
		}
	}
}
