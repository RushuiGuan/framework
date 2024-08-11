﻿using Albatross.CodeGen.Tests.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.WebApi {
	[Route("api/routing-param-test")]
	[Route("api/multiple-route-test")]
	[Route("api/[controller]")]
	public class RoutingParamTestController : ControllerBase {
		[HttpGet("1/{name}/{id}")]
		public Task Route1(string name, int id) => Task.CompletedTask;

		[HttpGet("2/{name}/{id}")]
		public Task Route2([FromRoute]string name, [FromRoute]int id) => Task.CompletedTask;

		[Route("3/{name}/{id}")]
		[Route("3.1/{name}/{id}")]
		[HttpGet("3.2/{name}/{id}")]
		public Task MultipleRouteTest([FromRoute] string name, [FromRoute] int id) => Task.CompletedTask;

		[HttpGet]
		[Route("wild-card-route/{**name}")]
		public Task Route4([FromRoute] string name) => Task.CompletedTask;

		[HttpGet("route-with-date-time/{date}/{id}")]
		public Task RouteWithDate([FromRoute] DateTime date, [FromRoute] int id) => Task.CompletedTask;

		[HttpGet("route-with-date-only/{date}/{id}")]
		public Task RouteWithDateOnly([FromRoute] DateOnly date, [FromRoute] int id) => Task.CompletedTask;
	}
}