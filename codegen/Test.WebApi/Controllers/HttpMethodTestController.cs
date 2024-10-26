using Microsoft.AspNetCore.Mvc;

namespace Test.WebApi.Controllers {
	[Route("api/http-method-test")]
	public class HttpMethodTestController : ControllerBase {
		[HttpDelete]
		public void Delete() { }

		[HttpPost]
		public void Post() { }

		[HttpPatch]
		public void Patch() { }

		[HttpGet]
		public int Get() => 0;

		[HttpPut]
		public void Put() { }
	}
}