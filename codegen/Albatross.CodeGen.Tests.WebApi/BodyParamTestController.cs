using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/body-param-test")]
	public class BodyParamTestController : ControllerBase {
		[HttpPost("post")]
		public Task FromBody1([FromBody] MyDto dto) => Task.CompletedTask;

		[HttpPost("post-with-nullable")]
		public Task FromBody2([FromBody] MyDto? dto) => Task.CompletedTask;

		[HttpPost("post-with-return")]
		public Task<MyClass> FromBody3([FromBody] MyDto dto) => Task.FromResult(new MyClass(string.Empty));
	}
}
