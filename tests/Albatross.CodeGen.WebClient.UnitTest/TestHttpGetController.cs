using Albatross.WebClient.Test.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.WebClient.UnitTest {
	[Route("api/test-get")]
	public class TestHttpGetController : ControllerBase {
		[HttpGet("object")]
		public Dto GetObject() => new Dto();

		[HttpGet("object-async")]
		public Task<Dto> GetObjectAsync() => Task.FromResult(new Dto());

		[HttpGet("void")]
		public void GetVoid() { }

		[HttpGet("task")]
		public Task GetTask() => Task.Delay(1);

		[HttpGet("string")]
		public string GetString() => DateTime.Now.ToString();

		[HttpGet("string-async")]
		public Task<string> GetStringAsync() => Task.FromResult(DateTime.Now.ToString());

		[HttpGet("route-only/{name}/{id}")]
		public Dto RouteOnly([FromRoute] string name, [FromRoute] int id) => new Dto { Name = name, Id = id, };

		[HttpGet("route-with-date/{date}/{id}")]
		public Dto RouteWithDate([FromRoute] DateTime date, [FromRoute] int id) => new Dto { Date = date, Id = id, };

		[HttpGet("query-string-only")]
		public Dto QueryStringOnly(string name, int id) => new Dto { Name = name, Id = id, };

		[HttpGet("query-string-with-date")]
		public Dto QueryStringWithDate(DateTime date, int id) => new Dto { Date = date, Id = id, };

		[HttpGet("mixed/{name}")]
		public Dto Mixed([FromRoute] string name, int id) => new Dto { Name = name, Id = id, };

		[HttpGet("mixed-dates/{tradeDate}")]
		public Dto MixedDates([FromRoute] DateTime tradeDate, DateTime settlementDate) => new Dto { Date = tradeDate, DateTimeOffset = settlementDate, };

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
		public Dto? TestNullableReferenceType([FromRoute] Dto? dto) {
			return dto;
		}
		[HttpGet("async-nullable-reference-type")]
		public Task<Dto?> TestAsyncNullableReferenceType([FromRoute] Dto? dto) {
			return Task.FromResult<Dto?>(dto);
		}
	}
}