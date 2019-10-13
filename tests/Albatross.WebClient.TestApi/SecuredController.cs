using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albatross.WebClient.IntegrationTest.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Albatross.WebClient.TestApi {
	[Authorize]
	[Route("api/[controller]")]
	public class SecuredController : Controller {
		[HttpGet("json")]
		public PayLoad GetJson() {
			return PayLoadExtension.Make();
		}
		[HttpGet("text")]
		public string GetText() {
			return PayLoadExtension.GetText();
		}

        [HttpPost]
        [Route("post")]
        public PayLoad Post([FromBody]PayLoad payLoad) {
            return payLoad;
        }
	}
}
