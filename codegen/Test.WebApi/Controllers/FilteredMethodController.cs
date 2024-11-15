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
		[HttpGet("csharp2")]
		public void FilteredByCSharp2() { }
		[HttpGet("include-this-method")]
		public void IncludedByCSharp() { }

		[HttpGet("typescript")]
		public void FilteredByTypeScript() { }
	}
}