using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Test.Dto;

namespace Test.WebApi.Controllers {
	[Route("api/nullable-param-test")]
	public class NullableParamTestController : ControllerBase {

		[HttpGet("required-string-param")]
		public string RequiredStringParam(string text) => text;

		[HttpGet("nullable-string-param")]
		public string NullableStringParam(string? text) => text ?? string.Empty;

		[HttpGet("required-value-type")]
		public string RequiredValueType(int id) => id.ToString();

		[HttpGet("nullable-value-type")]
		public string NullableValueType(int? id) => id?.ToString() ?? string.Empty;

		[HttpGet("required-date-only")]
		public string RequiredDateOnly(DateOnly date) => date.ToString();

		[HttpGet("nullable-date-only")]
		public string NullableDateOnly(DateOnly? date) => date?.ToString() ?? string.Empty;

		[HttpPost("nullable-post-param")]
		public void NullablePostParam([FromBody] MyDto? dto) { }

		[HttpPost("required-post-param")]
		public void RequiredPostParam([FromBody] MyDto dto) { }

		[HttpGet("required-string-array")]
		public string RequiredStringArray(string[] values) => string.Join(",", values);

		[HttpGet("required-string-collection")]
		public string RequiredStringCollection([FromQuery] IEnumerable<string> values) => string.Join(",", values);

		[HttpGet("nullable-string-array")]
		public string NullableStringArray(string?[] values) => string.Join(",", values);

		[HttpGet("nullable-string-collection")]
		public string NullableStringCollection([FromQuery] IEnumerable<string?> values) => string.Join(",", values);

		[HttpGet("required-value-type-array")]
		public string RequiredValueTypeArray(int[] values) => string.Join(",", values);

		[HttpGet("required-value-type-collection")]
		public string RequiredValueTypeCollection([FromQuery] IEnumerable<int> values) => string.Join(",", values);

		[HttpGet("nullable-value-type-array")]
		public string NullableValueTypeArray(int?[] values) => string.Join(",", values);

		[HttpGet("nullable-value-type-collection")]
		public string NullableValueTypeCollection([FromQuery] IEnumerable<int?> values) => string.Join(",", values);
	}
}