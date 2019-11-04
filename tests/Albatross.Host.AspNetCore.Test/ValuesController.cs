using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Albatross.Host.AspNetCore.Test {
	[Route("api/[controller]")]
	public class ValuesController : Controller {
		[HttpGet]
		public IEnumerable<string> Get() {
			return new string[] { "value1", "value2" };
		}

		[HttpGet("test-error")]
		public IEnumerable<string> TestException() {
			throw new Exception("test");
		}
	}
}
