using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/method-test")]
	public class MethodTestController : ControllerBase {
		[HttpDelete]
		public void Delete() { }
		
		[HttpPost]
		public void Post() { }

		[HttpPost("string")]
		public string PostAndReturnString() => string.Empty;

		[HttpPatch]
		public void Patch() { }

		[HttpPatch("string")]
		public string PatchAndReturnString() => string.Empty;

		[HttpGet]
		public int Get() => 0;

		[HttpGet("string")]
		public string GetAndReturnString() => string.Empty;

		[HttpPut]
		public void Put() { }

		[HttpPut("string")]
		public string PutAndReturnString() => string.Empty;
	}
}
