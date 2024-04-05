using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/test-get")]
	public class TestHttpGetController : ControllerBase {
		[HttpGet("object")]
		public MyDto GetObject() => new MyDto();

		[HttpGet("object-async")]
		public Task<MyDto> GetObjectAsync() => Task.FromResult(new MyDto());

		[HttpGet("void")]
		public void GetVoid() { }

		[HttpGet("task")]
		public Task GetTask() => Task.Delay(1);

		[HttpGet("string")]
		public string GetString() => DateTime.Now.ToString();

		[HttpGet("string-async")]
		public Task<string> GetStringAsync() => Task.FromResult(DateTime.Now.ToString());

		[HttpGet("route-only/{name}/{id}")]
		public MyDto RouteOnly([FromRoute] string name, [FromRoute] int id) => new MyDto { Name = name, Id = id, };

		[HttpGet("route-with-date/{date}/{id}")]
		public MyDto RouteWithDate([FromRoute] DateTime date, [FromRoute] int id) => new MyDto { Date = date, Id = id, };

		[HttpGet("query-string-only")]
		public MyDto QueryStringOnly(string name, int id) => new MyDto { Name = name, Id = id, };

		[HttpGet("query-string-with-date")]
		public MyDto QueryStringWithDate(DateTime date, int id) => new MyDto { Date = date, Id = id, };

		[HttpGet("mixed/{name}")]
		public MyDto Mixed([FromRoute] string name, int id) => new MyDto { Name = name, Id = id, };

		[HttpGet("mixed-dates/{tradeDate}")]
		public MyDto MixedDates([FromRoute] DateTime tradeDate, DateTime settlementDate) => new MyDto { Date = tradeDate, DateTimeOffset = settlementDate, };

		[HttpGet("array-query-string")]
		public string TestArrayInput([FromQuery] string[] items) {
			return string.Join(",", items);
		}

		[HttpGet("wild-card-route-param/{**name}")]
		public string TestWildCardRoute([FromRoute] string name) {
			return name;
		}

		[HttpGet("nullable-value-type")]
		public int? TestNullableValueType([FromRoute] int? id) {
			return id;
		}

		[HttpGet("async-nullable-value-type")]
		public Task<int?> TestAsyncNullableValueType([FromRoute] int? id) {
			return Task.FromResult(id);
		}

		[HttpGet("nullable-reference-type")]
		public MyDto? TestNullableReferenceType([FromRoute] MyDto? dto) {
			return dto;
		}
		[HttpGet("async-nullable-reference-type")]
		public Task<MyDto?> TestAsyncNullableReferenceType([FromRoute] MyDto? dto) {
			return Task.FromResult<MyDto?>(dto);
		}

		[HttpGet("plain-action-result")]
		public ActionResult<MyDto> TestPlainActionResult() {
			return Ok(new MyDto());
		}

		[HttpGet("async-plain-action-result")]
		public async Task<ActionResult<MyDto>> AsyncActionResult() {
			await Task.Delay(1);
			return Ok(new MyDto());
		}
	}
}