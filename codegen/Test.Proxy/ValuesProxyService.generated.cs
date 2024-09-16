using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class ValuesProxyService : ClientBase {
		public ValuesProxyService(ILogger<ValuesProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/values";
	}
}
#nullable disable

