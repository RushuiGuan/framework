using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Hosting.Demo.Controllers {
public class Input {
		public string Name { get; set; }
		public int Id { get; set; }
		public string Now => DateTime.Now.Ticks.ToString();
	}
	[Route("home")]
	public class HomeController : Controller {
		[HttpPost]
		public ActionResult Details([FromBody]Input input) {
			return View("my very special view.default-super", input);
		}

		[HttpGet]
		public ActionResult Get([FromQuery]int id) {
			return View("my very special view.default-super", new Input { Id = id});
		}

		[HttpPost("text")]
		public string Post([FromBody]string text) {
			return text;
		}
	}
}
