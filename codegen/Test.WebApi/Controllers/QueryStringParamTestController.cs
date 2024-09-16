using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.WebApi.Controllers {
	[Route("api/query-string-param-test")]
	public class QueryStringParamTestController : ControllerBase {
		[HttpGet]
		public Task<string> Get([FromQuery] string name, int id) => Task.FromResult("");

		[HttpGet("test-nullable-params")]
		public Task<string> GetWithNullableParam([FromQuery] string? name, int? id) => Task.FromResult("");

		[HttpGet("test-datetime")]
		public Task<string> TestDateTime(DateTime dateTime) => Task.FromResult("");

		[HttpGet("test-dateOnly")]
		public Task<string> TestDateOnly(DateOnly date) => Task.FromResult("");

		[HttpGet("test-datetimeoffset")]
		public Task<string> TestDateOnly(DateTimeOffset dateTimeOffset) => Task.FromResult("");

		[HttpGet("array-query-string")]
		public Task<string> TestArrayInput([FromQuery] string[] items) => Task.FromResult("");

		[HttpGet("ienumerable-generic-query-string")]
		public Task<string> TestIEnumerableGenericInput([FromQuery] IEnumerable<string> items) => Task.FromResult("");

		[HttpGet("ienumerable-query-string")]
		public Task<string> TestIEnumerableInput([FromQuery] IEnumerable items) => Task.FromResult("");

		[HttpGet("query-string-with-diff-name")]
		public Task<string> TestQueryName([FromQuery(Name = "i")] string[] items) => Task.FromResult("");
	}
}