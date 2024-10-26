using Microsoft.AspNetCore.Mvc;

namespace Test.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ControllerRouteTestController : ControllerBase {
		[HttpPost]
		public void Post() {
		}
	}
}