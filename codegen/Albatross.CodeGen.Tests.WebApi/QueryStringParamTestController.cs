using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/query-string-param-test")]
	public class QueryStringParamTestController : ControllerBase {
		[HttpGet]
		public Task Get([FromQuery]string name, int id) => Task.CompletedTask;

		[HttpGet("nullable")]
		public Task GetWithNullableParam([FromQuery] string? name, int? id) => Task.CompletedTask;

		[HttpGet("test-datetime")]
		public Task TestDateTime(DateTime dateTime) => Task.CompletedTask;

		[HttpGet("test-dateOnly")]
		public Task TestDateOnly(DateOnly date) => Task.CompletedTask;

		[HttpGet("test-datetimeoffset")]
		public Task TestDateOnly(DateTimeOffset dateTimeOffset) => Task.CompletedTask;

		[HttpGet("array-query-string")]
		public Task TestArrayInput([FromQuery] string[] items) => Task.CompletedTask;

		[HttpGet("query-string-name")]
		public Task TestQueryName([FromQuery(Name ="i")] string[] items) => Task.CompletedTask;
	}
}