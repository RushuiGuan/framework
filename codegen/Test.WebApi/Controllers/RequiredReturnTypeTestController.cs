using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Dto.Classes;

namespace Test.WebApi.Controllers {
	[Route("api/required-return-type")]
	public class RequiredReturnTypeTestController : ControllerBase {
		[HttpGet("void")]
		public void Get() { }

		[HttpGet("async-task")]
		public Task GetAsync() => Task.CompletedTask;

		[HttpGet("action-result")]
		public ActionResult GetActionResult() => Ok();

		[HttpGet("async-action-result")]
		public Task<ActionResult> GetAsyncActionResult() => Task.FromResult<ActionResult>(Ok());

		[HttpGet("string")]
		public string GetString() => string.Empty;

		[HttpGet("async-string")]
		public Task<string> GetAsyncString() => Task.FromResult(string.Empty);

		[HttpGet("action-result-string")]
		public ActionResult<string> GetActionResultString() => Ok(string.Empty);

		[HttpGet("async-action-result-string")]
		public Task<ActionResult<string>> GetAsyncActionResultString() => Task.FromResult<ActionResult<string>>(Ok(string.Empty));

		[HttpGet("int")]
		public int GetInt() => 0;

		[HttpGet("async-int")]
		public Task<int> GetAsyncInt() => Task.FromResult(0);

		[HttpGet("action-result-int")]
		public ActionResult<int> GetActionResultInt() => Ok(0);

		[HttpGet("async-action-result-int")]
		public Task<ActionResult<int>> GetAsyncActionResultInt() => Task.FromResult<ActionResult<int>>(Ok(0));

		[HttpGet("datetime")]
		public DateTime GetDateTime() => DateTime.Now;

		[HttpGet("async-datetime")]
		public Task<DateTime> GetAsyncDateTime() => Task.FromResult(DateTime.Now);

		[HttpGet("action-result-datetime")]
		public ActionResult<DateTime> GetActionResultDateTime() => Ok(DateTime.Now);

		[HttpGet("async-action-result-datetime")]
		public Task<ActionResult<DateTime>> GetAsyncActionResultDateTime() => Task.FromResult<ActionResult<DateTime>>(Ok(DateTime.Now));

		[HttpGet("dateonly")]
		public DateOnly GetDateOnly() => DateOnly.FromDateTime(DateTime.Now);

		[HttpGet("datetimeoffset")]
		public DateTimeOffset GetDateTimeOffset() => DateTime.Now;

		[HttpGet("timeonly")]
		public TimeOnly GetTimeOnly() => TimeOnly.MinValue;

		[HttpGet("object")]
		public MyDto GetMyDto() => new MyDto();

		[HttpGet("async-object")]
		public Task<MyDto> GetAsyncMyDto() => Task.FromResult(new MyDto());

		[HttpGet("action-result-object")]
		public ActionResult<MyDto> ActionResultObject() {
			return Ok(new MyDto());
		}

		[HttpGet("async-action-result-object")]
		public async Task<ActionResult<MyDto>> AsyncActionResultObject() {
			await Task.Delay(1);
			return Ok(new MyDto());
		}

		[HttpGet("array-return-type")]
		public MyDto[] GetMyDtoArray() => Array.Empty<MyDto>();

		[HttpGet("collection-return-type")]
		public IEnumerable<MyDto> GetMyDtoCollection() => Array.Empty<MyDto>();

		[HttpGet("async-collection-return-type")]
		public IAsyncEnumerable<MyDto> GetMyDtoCollectionAsync() => AsyncEnumerable.Empty<MyDto>();
	}
}