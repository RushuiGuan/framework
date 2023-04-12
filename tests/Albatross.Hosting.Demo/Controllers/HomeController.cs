using Albatross.Caching;
using Albatross.Hosting.Demo.CacheMgmt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Hosting.Demo.Controllers {
	public class Input {
		public string? Name { get; set; }
		public int Id { get; set; }
		public string Now => DateTime.Now.Ticks.ToString();
	}
	[Route("home")]
	public class HomeController : Controller {
		private readonly ICacheManagementFactory cacheManagementFactory;

		public HomeController(ICacheManagementFactory cacheManagementFactory) {
			this.cacheManagementFactory = cacheManagementFactory;
		}
		[HttpPost]
		public ActionResult Details([FromBody] Input input) {
			return View("my very special view.default-super", input);
		}

		[HttpGet]
		public ActionResult Get([FromQuery] int id) {
			return View("my very special view.default-super", new Input { Id = id });
		}

		[HttpPost("text")]
		public string Post([FromBody] string text) {
			return text;
		}

		[HttpGet("symbol")]
		public Task<IEnumerable<string>> GetSymbol() {
			var cacheMgmt = cacheManagementFactory.Get<IEnumerable<string>>(SymbolCacheManagement.CacheName);
			return cacheMgmt.ExecuteAsync(context => Task.FromResult<IEnumerable<string>>(new string[] {
				"a", "b","c",
			}), new Polly.Context());
		}

		static int pollyTest = 1;

		[HttpGet("polly-test")]
		public string RunPollyTest([FromQuery]int count) {
			try {
				if (pollyTest % count != 0) {
					throw new Exception($"Polly test returns error: {pollyTest}");
				} else {
					return "successful";
				}
			} finally {
				pollyTest++;
			}
		}

		[HttpPost("polly-post-test")]
		public async Task<MyResponse> PostData([FromBody] MyRequest myRequest) {
			if (myRequest.Input == 0) {
				throw new System.Exception($"input cannot be 0");
			} else if (myRequest.Input == 1) {
				await Task.Delay(10000);
			} else if (myRequest.Input == 2) {
				return new MyResponse {
					Success = false,
					Output = myRequest.Input,
				};
			}
			return new MyResponse {
				Success = true,
				Output = myRequest.Input,
			};
		}
	}
	public class MyRequest {
		public int Input { get; set; }
		public string Data { get; set; }
		public MyRequest(string data) {
			this.Data = data;
		}
	}

	public class MyResponse {
		public int Output { get; set; }
		public bool Success { get; set; }
	}
}