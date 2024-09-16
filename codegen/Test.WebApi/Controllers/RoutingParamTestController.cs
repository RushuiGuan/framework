using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Test.WebApi.Controllers {
	[Route("api/routing-param-test")]
	[Route("api/multiple-route-test")]
	[Route("api/[controller]")]
	public class RoutingParamTestController : ControllerBase {
		private readonly ILogger logger;

		public RoutingParamTestController(ILogger logger) {
			this.logger = logger;
		}

		[HttpGet("1/{name}/{id}")]
		public Task Route1(string name, int id) => Task.CompletedTask;

		[HttpGet("2/{name}/{id}")]
		public Task Route2([FromRoute] string name, [FromRoute] int id) => Task.CompletedTask;

		[Route("3/{name}/{id}")]
		[Route("3.1/{name}/{id}")]
		[HttpGet]
		public Task MultipleRouteTest([FromRoute] string name, [FromRoute] int id) => Task.CompletedTask;

		[HttpGet]
		[Route("wild-card-route/{*name}")]
		public Task Route4(string name, int id) => Task.CompletedTask;

		[HttpGet]
		[Route("wild-card-route2/{**name}")]
		public Task Route5(string name, int id) => Task.CompletedTask;

		[HttpGet("route-with-date-time/{date}/{id}")]
		public Task RouteWithDateTime([FromRoute] DateTime date, [FromRoute] int id) => Task.CompletedTask;

		[HttpGet("route-with-date-time-as-date-only/{date}/{id}")]
		public Task RouteWithDateTimeAsDateOnly([FromRoute] DateTime date, [FromRoute] int id) => Task.CompletedTask;

		[HttpGet("route-with-date-only/{date}/{id}")]
		public Task RouteWithDateOnly([FromRoute] DateOnly date, [FromRoute] int id) => Task.CompletedTask;

		[HttpGet("route-without-attribute/{date}")]
		public Task RouteWithoutRouteAttribute(DateOnly date) => Task.CompletedTask;
	}
}

