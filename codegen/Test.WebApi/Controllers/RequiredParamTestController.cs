using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Test.Dto.Classes;

namespace Test.WebApi.Controllers {
	[Route("api/required-param-test")]
	public class RequiredParamTestController : ControllerBase {

		[HttpGet("explicit-string-param")]
		public string ExplicitStringParam([FromQuery] string text) => text;

		[HttpGet("implicit-string-param")]
		public string ImplicitStringParam(string text) => text;

		[HttpGet("required-string-param")]
		public string RequiredStringParam(string text) => text;

		[HttpGet("required-value-type")]
		public string RequiredValueType(int id) => id.ToString();

		[HttpGet("required-date-only")]
		public string RequiredDateOnly(DateOnly date) => date.ToString();

		[HttpGet("required-datetime")]
		public string RequiredDateTime(DateTime date) => date.ToString();

		[HttpGet("requried-datetime-as-dateonly")]
		public string RequiredDateTimeAsDateOnly(DateTime date) => date.ToString();

		[HttpPost("required-post-param")]
		public void RequiredPostParam([FromBody] MyDto dto) { }

		[HttpGet("required-string-array")]
		public string RequiredStringArray(string[] values) => string.Join(",", values);

		[HttpGet("required-string-collection")]
		public string RequiredStringCollection([FromQuery] IEnumerable<string> values) => string.Join(",", values);

		[HttpGet("required-value-type-array")]
		public string RequiredValueTypeArray(int[] values) => string.Join(",", values);

		[HttpGet("required-value-type-collection")]
		public string RequiredValueTypeCollection([FromQuery] IEnumerable<int> values) => string.Join(",", values);

		[HttpGet("required-date-only-collection")]
		public string RequiredDateOnlyCollection([FromQuery] IEnumerable<DateOnly> dates) => string.Join(",", dates);

		[HttpGet("required-date-only-array")]
		public string RequiredDateOnlyArray([FromQuery] DateOnly[] dates) => string.Join(",", dates);

		[HttpGet("required-datetime-collection")]
		public string RequiredDateTimeCollection([FromQuery] IEnumerable<DateTime> dates) => string.Join(",", dates);

		[HttpGet("required-datetime-array")]
		public string RequiredDateTimeArray([FromQuery] DateTime[] dates) => string.Join(",", dates);

		[HttpGet("required-datetime-as-dateonly-collection")]
		public string RequiredDateTimeAsDateOnlyCollection([FromQuery] IEnumerable<DateTime> dates) => string.Join(",", dates);

		[HttpGet("required-datetime-as-dateonly-array")]
		public string RequiredDateTimeAsDateOnlyArray([FromQuery] DateTime[] dates) => string.Join(",", dates);
	}
}