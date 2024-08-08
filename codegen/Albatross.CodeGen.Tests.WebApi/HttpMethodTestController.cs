using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/http-method-test")]
	public class HttpMethodTestController : ControllerBase {
		[HttpDelete]
		public void Delete() { }
		
		[HttpPost]
		public void Post() { }

		[HttpPost("post-and-return-string")]
		public string PostAndReturnString() => string.Empty;

		[HttpPatch]
		public void Patch() { }

		[HttpPatch("patch-and-return-string")]
		public string PatchAndReturnString() => string.Empty;

		[HttpGet]
		public int Get() => 0;

		[HttpGet("get-and-return-string")]
		public string GetAndReturnString() => string.Empty;

		[HttpPut]
		public void Put() { }

		[HttpPut("put-and-return-string")]
		public string PutAndReturnString() => string.Empty;
	}
}
