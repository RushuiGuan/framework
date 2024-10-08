using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Sample.Proxy {
	public partial class CommandProxyService : ClientBase {
		public CommandProxyService(ILogger<CommandProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/command";
		public async Task SubmitSystemCommand(Sample.Core.Commands.MyOwnNameSpace.ISystemCommand systemCommand) {
			string path = $"{ControllerPath}/system";
			var queryString = new NameValueCollection();
			queryString.Add("systemCommand", $"{systemCommand}");
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task SubmitAppCommand(Sample.Core.Commands.MyOwnNameSpace.IApplicationCommand applicationCommand) {
			string path = $"{ControllerPath}/app";
			var queryString = new NameValueCollection();
			queryString.Add("applicationCommand", $"{applicationCommand}");
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

