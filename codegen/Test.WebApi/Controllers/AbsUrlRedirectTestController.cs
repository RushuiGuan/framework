using Microsoft.AspNetCore.Mvc;
using System;

namespace Test.WebApi.Controllers {
	[Route("api/abs-url-redirect-test")]
	[ApiController]
	public class AbsUrlRedirectTestController : ControllerBase {

		string GetRedirectUrl(string action) {
			return $"{Request.Scheme}://{Request.Host}/api/abs-url-redirect-test/{action}";
		}

		[HttpGet("test-0")]
		public void Get() {
			Response.Redirect(GetRedirectUrl("test-1"));
		}

		[HttpGet("test-1")]
		public void Get1() {
			Response.Redirect(GetRedirectUrl("test-2"));
		}

		[HttpGet("test-2")]
		public void Get2() {
			Response.Redirect(GetRedirectUrl("test-3"));
		}

		[HttpGet("test-3")]
		public void Get3() {
			Response.Redirect(GetRedirectUrl("test-4"));
		}

		[HttpGet("test-4")]
		public void Get4() {
			Response.Redirect(GetRedirectUrl("test-5"));
		}

		[HttpGet("test-5")]
		public void Get5() {
			Response.Redirect(GetRedirectUrl("test-6"));
		}

		[HttpGet("test-6")]
		public void Get6() {
			Response.Redirect(GetRedirectUrl("test-7"));
		}

		[HttpGet("test-7")]
		public void Get7() {
			Response.Redirect(GetRedirectUrl("test-8"));
		}

		[HttpGet("test-8")]
		public void Get8() {
			Response.Redirect(GetRedirectUrl("test-9"));
		}

		[HttpGet("test-9")]
		public void Get9() {
			Response.Redirect(GetRedirectUrl("test-10"));
		}

		[HttpGet("test-10")]
		public string Get10() {
			return "Here is the end of the redirect chain";
		}
	}
}