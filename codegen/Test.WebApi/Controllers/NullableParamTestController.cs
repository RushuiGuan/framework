using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Test.Dto.Classes;

namespace Test.WebApi.Controllers {
	[Route("api/nullable-param-test")]
	public class NullableParamTestController : ControllerBase {
		[HttpGet("nullable-string-param")]
		public string NullableStringParam(string? text) => text ?? string.Empty;

		[HttpGet("nullable-value-type")]
		public string NullableValueType(int? id) => id?.ToString() ?? string.Empty;

		[HttpGet("nullable-date-only")]
		public string NullableDateOnly(DateOnly? date) => date?.ToString() ?? string.Empty;

		[HttpPost("nullable-post-param")]
		public void NullablePostParam([FromBody] MyDto? dto) { }

		[HttpGet("nullable-string-array")]
		public string NullableStringArray(string?[] values) => string.Join(",", values);

		[HttpGet("nullable-string-collection")]
		public string NullableStringCollection([FromQuery] IEnumerable<string?> values) => string.Join(",", values);

		[HttpGet("nullable-value-type-array")]
		public string NullableValueTypeArray(int?[] values) => string.Join(",", values);

		[HttpGet("nullable-value-type-collection")]
		public string NullableValueTypeCollection([FromQuery] IEnumerable<int?> values) => string.Join(",", values);

		[HttpGet("nullable-date-only-collection")]
		public string NullableDateOnlyCollection([FromQuery] IEnumerable<DateOnly?> dates) => string.Join(",", dates);

		[HttpGet("nullable-date-only-array")]
		public string NullableDateOnlyArray([FromQuery] DateOnly?[] dates) => string.Join(",", dates);

	}
}