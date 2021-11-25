using Albatross.WebClient.Test.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Albatross.CodeGen.WebClient.UnitTest {
	[Route("api/test-post")]
	public class TestHttpPostController : ControllerBase {
		[HttpPost("from-body")]
		public Dto FromBody([FromBody]Dto dto) => dto;

		[HttpPost("post-string")]
		public string PostStringOnly([FromBody] string body) => body;

		[HttpPost("query-string")]
		public string QueryString(string name) => name;

		[HttpPost("route-param/{name}")]
		public string RouteParam(string name) => name;

		[HttpPost("mixed/{name}")]
		public string Mixed([FromRoute]string name, int id, [FromBody]Dto dto) => name;

		[HttpPost("async-void")]
		public async void AsyncVoid(int i) {
			await Task.Delay(i);
		}
	}
}
