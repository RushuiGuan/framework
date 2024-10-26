using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Dto.Classes;

namespace Test.WebApi.Controllers {
	[Route("api/nullable-return-type")]
	public class NullableReturnTypeTestController : ControllerBase {
		[HttpGet("string")]
		public string? GetString() => string.Empty;

		[HttpGet("async-string")]
		public Task<string?> GetAsyncString() => Task.FromResult<string?>(string.Empty);

		[HttpGet("action-result-string")]
		public ActionResult<string?> GetActionResultString() => Ok(string.Empty);

		[HttpGet("async-action-result-string")]
		public Task<ActionResult<string?>> GetAsyncActionResultString() => Task.FromResult<ActionResult<string?>>(Ok(string.Empty));

		[HttpGet("int")]
		public int? GetInt() => 0;

		[HttpGet("async-int")]
		public Task<int?> GetAsyncInt() => Task.FromResult<int?>(0);

		[HttpGet("action-result-int")]
		public ActionResult<int?> GetActionResultInt() => Ok(0);

		[HttpGet("async-action-result-int")]
		public Task<ActionResult<int?>> GetAsyncActionResultInt() => Task.FromResult<ActionResult<int?>>(Ok(0));

		[HttpGet("datetime")]
		public DateTime? GetDateTime() => DateTime.Now;

		[HttpGet("async-datetime")]
		public Task<DateTime?> GetAsyncDateTime() => Task.FromResult<DateTime?>(DateTime.Now);

		[HttpGet("action-result-datetime")]
		public ActionResult<DateTime?> GetActionResultDateTime() => Ok(DateTime.Now);

		[HttpGet("async-action-result-datetime")]
		public Task<ActionResult<DateTime?>> GetAsyncActionResultDateTime() => Task.FromResult<ActionResult<DateTime?>>(Ok(DateTime.Now));

		[HttpGet("object")]
		public MyDto? GetMyDto() => new MyDto();

		[HttpGet("async-object")]
		public Task<MyDto?> GetAsyncMyDto() => Task.FromResult<MyDto?>(new MyDto());

		[HttpGet("action-result-object")]
		public ActionResult<MyDto?> ActionResultObject() {
			return Ok(new MyDto());
		}

		[HttpGet("async-action-result-object")]
		public async Task<ActionResult<MyDto?>> AsyncActionResultObject() {
			await Task.Delay(1);
			return Ok(new MyDto());
		}

		[HttpGet("nullable-array-return-type")]
		public MyDto?[] GetMyDtoNullableArray() => Array.Empty<MyDto>();

		[HttpGet("nullable-collection-return-type")]
		public IEnumerable<MyDto?> GetMyDtoCollection() => Array.Empty<MyDto>();
	}
}