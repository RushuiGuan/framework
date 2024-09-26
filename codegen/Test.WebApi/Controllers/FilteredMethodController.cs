using Microsoft.AspNetCore.Mvc;

namespace Test.WebApi.Controllers {
	[Route("api/filtered-method")]
	[ApiController]
	public class FilteredMethodController : ControllerBase {
		[HttpGet("all")]
		public void FilteredByAll() { }
		[HttpGet("none")]
		public void FilteredByNone() { }
		[HttpGet("csharp")]
		public void FilteredByCSharp() { }
		[HttpGet("typescript")]
		public void FilteredByTypeScript() { }
	}
}
