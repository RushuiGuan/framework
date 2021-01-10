using Albatross.WebClient.IntegrationTest.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Albatross.WebClient.TestApi.Controllers {
	[Route("api/[controller]")]
		public class ValueController : Controller {
		private readonly DatabaseSetting setting;

		public ValueController(DatabaseSetting setting) {
			this.setting = setting;
		}


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

		[HttpGet("config")]
		public string GetConfig() {
			return setting.ConnectionString;
		}

		[HttpGet("timeout")]
		public async Task TimeoutInSeconds([FromQuery]int seconds) {
			await Task.Delay(seconds * 1000);
		}
	}
}
