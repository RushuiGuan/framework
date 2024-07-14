using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/method-test")]
	public class MethodTestController : ControllerBase {
		[HttpDelete]
		public void Delete() { }
		
		[HttpPost]
		public void Post() { }
		
		[HttpPatch]
		public void Patch() { }
	
		[HttpGet]
		public void Get() { }

		[HttpPut]
		public void Put() { }
 	}
}
