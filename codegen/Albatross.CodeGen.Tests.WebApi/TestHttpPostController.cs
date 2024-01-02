using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/test-post")]
	public class TestHttpPostController : ControllerBase {
		[HttpPost("from-body")]
		public MyDto FromBody([FromBody] MyDto dto) => dto;

		[HttpPost("post-string")]
		public string PostStringOnly([FromBody] string body) => body;

		[HttpPost("query-string")]
		public string QueryString(string name) => name;

		[HttpPost("named-query-string")]
		public string NamedQueryString([FromQuery(Name = "n")] string name) => name;

		[HttpPost("route-param/{name}")]
		public string RouteParam(string name) => name;

		[HttpPost("mixed/{name}")]
		public string Mixed([FromRoute] string name, int id, [FromBody] MyDto dto) => name;

		[HttpPost("async-void")]
		public async void AsyncVoid(int i) {
			await Task.Delay(i);
		}
	}
}
