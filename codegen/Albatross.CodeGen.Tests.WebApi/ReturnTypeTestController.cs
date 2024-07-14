using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/return-type-test")]
	public class ReturnTypeTestController : ControllerBase {
		[HttpGet("void")]
		public void Get() { }

		[HttpGet("string")]
		public string GetString() => string.Empty;

		[HttpGet("my-dto")]
		public MyDto GetMyDto() => new MyDto();

		[HttpGet("async")]
		public Task GetAsync() => Task.CompletedTask;

		[HttpGet("async-string")]
		public Task<string> GetAsyncString() => Task.FromResult(string.Empty);

		[HttpGet("async-nullable-string")]
		public Task<string?> GetAsyncNullableString() => Task.FromResult<string?>(string.Empty);

		[HttpGet("async-nullable-int")]
		public Task<int?> GetAsyncNullableInt() => Task.FromResult<int?>(1);

		[HttpGet("async-my-dto")]
		public Task<MyDto> GetAsyncMyDto() => Task.FromResult(new MyDto());

		[HttpGet("async-my-dto-nullable")]
		public Task<MyDto?> GetAsyncMyDtoNullable() => Task.FromResult<MyDto?>(new MyDto());

		[HttpGet("action-result-generic")]
		public ActionResult<MyDto> ActionResult() {
			return Ok(new MyDto());
		}

		[HttpGet("async-action-result-generic")]
		public async Task<ActionResult<MyDto>> AsyncActionResult() {
			await Task.Delay(1);
			return Ok(new MyDto());
		}

		[HttpGet("action-result-any")]
		public ActionResult ActionResultAny() {
			return Ok();
		}
		 
		[HttpGet("async-action-result-any")]
		public async Task<ActionResult> AsyncActionResultAny() {
			await Task.Delay(1);
			return Ok(new MyDto());
		}
	}
}
