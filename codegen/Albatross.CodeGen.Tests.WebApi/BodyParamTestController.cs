using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/body-param-test")]
	public class BodyParamTestController : ControllerBase {
		[HttpPost]
		public Task FromBody([FromBody] MyDto dto) => Task.CompletedTask;

		[HttpPost]
		public Task FromBodyNullable([FromBody] MyDto? dto) => Task.CompletedTask;
	}
}
