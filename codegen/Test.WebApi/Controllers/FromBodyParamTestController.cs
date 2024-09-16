using Test.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Test.WebApi.Controllers {
	[Route("api/from-body-param-test")]
	public class FromBodyParamTestController : ControllerBase {
		[HttpPost("required-object")]
		public void RequiredObject([FromBody] MyDto dto) { }

		[HttpPost("nullable-object")]
		public void NullableObject([FromBody] MyDto? dto) { }

		[HttpPost("required-int")]
		public void RequiredInt([FromBody] int value) { }

		[HttpPost("nullable-int")]
		public void NullableInt([FromBody] int? value) { }

		[HttpPost("required-string")]
		public void RequiredString([FromBody] string value) { }

		[HttpPost("nullable-string")]
		public void NullableString([FromBody] string? value) { }

		[HttpPost("required-object-array")]
		public void RequiredObjectArray([FromBody] MyDto[] array) { }

		[HttpPost("nullable-object-array")]
		public void NullableObjectArray([FromBody] MyDto?[] array) { }
	}
}