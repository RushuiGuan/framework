using Albatross.WebClient.Test.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.WebClient.UnitTest {
	[Route("api/test-delete")]
	public class TestHttpDeleteController : ControllerBase {
		[HttpDelete("route-only/{name}/{id}")]
		public void RouteOnly([FromRoute] string name, [FromRoute] int id) { }

		[Route("route-with-date/{date}/{id}")]
		[HttpDelete]
		public Task RouteWithDate([FromRoute] DateTime date, [FromRoute] int id) => Task.CompletedTask;

		[Route("query-string-only")]
		[HttpDelete]
		public void QueryStringOnly(string name, int id) { }

		[Route("query-string-with-date")]
		[HttpDelete]
		public void QueryStringWithDate(DateTime date, int id) { }

		[Route("mixed/{name}")]
		[HttpDelete]
		public void Mixed([FromRoute] string name, int id) { }

		[Route("mixed-dates/{tradeDate}")]
		[HttpDelete]
		public void MixedDates([FromRoute] DateTime tradeDate, DateTime settlementDate) { }

		[Route("array-query-string")]
		[HttpDelete]
		public Task TestArrayInput([FromQuery] string[] items) => Task.CompletedTask;

		[Route("wild-card-route-param/{**name}")]
		[HttpDelete]
		public Task TestWildCardRoute([FromRoute] string name) => Task.CompletedTask;
 	}
}
