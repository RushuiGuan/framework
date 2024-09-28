using Microsoft.AspNetCore.Mvc;
using Test.Dto.Classes;

namespace Test.WebApi.Controllers {
	[Route("api/from-body-param-test")]
	public class FromBodyParamTestController : ControllerBase {
		[HttpPost("required-object")]
		public int RequiredObject([FromBody] MyDto dto) => 1;

		[HttpPost("nullable-object")]
		public int NullableObject([FromBody] MyDto? dto) => 1;

		[HttpPost("required-int")]
		public int RequiredInt([FromBody] int value) => 1;

		[HttpPost("nullable-int")]
		public int NullableInt([FromBody] int? value) => 1;

		[HttpPost("required-string")]
		public int RequiredString([FromBody] string value) => 1;

		[HttpPost("nullable-string")]
		public int NullableString([FromBody] string? value) => 1;

		[HttpPost("required-object-array")]
		public int RequiredObjectArray([FromBody] MyDto[] array) => 1;

		[HttpPost("nullable-object-array")]
		public int NullableObjectArray([FromBody] MyDto?[] array) => 1;
	}
}