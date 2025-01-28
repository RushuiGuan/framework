using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class CustomJsonSettingsProxyService : ClientBase {
		public CustomJsonSettingsProxyService(ILogger<CustomJsonSettingsProxyService> logger, HttpClient client) : base(logger, client, MyCustomJsonSettings.Instance) {
		}

		public const string ControllerPath = "api/customjsonsettings";
	}
}
#nullable disable

