using Albatross.WebClient.IntegrationTest.Messages;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Albatross.CodeGen.WebClient.UnitTest {
	[Route("api/[controller]")]
	public class ValueController : ControllerBase {
		[HttpGet("json")]
		public PayLoad GetJson() {
			return PayLoadExtension.Make();
		}
		[HttpGet("text")]
		public string GetText() {
			return PayLoadExtension.GetText();
		}

        [HttpPost]
        [Route("post")]
        public PayLoad Post([FromBody]PayLoad payLoad) {
            return payLoad;
        }
	}
}
