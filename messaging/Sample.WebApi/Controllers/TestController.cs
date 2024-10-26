using Microsoft.AspNetCore.Mvc;

namespace Sample.WebApi.Controllers {
	[Route("api/test")]
	[ApiController]
	public class TestController : ControllerBase {
		public TestController() {
		}
		[HttpPost("plain-text-post")]
		public string TestPostPlainTextToBody([FromBody] string text) => text;
	}
}