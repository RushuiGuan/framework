using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class ControllerRouteTestProxyService : ClientBase {
		public ControllerRouteTestProxyService(ILogger<ControllerRouteTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/controllerroutetest";
	}
}
#nullable disable

