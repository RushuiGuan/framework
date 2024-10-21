using Albatross.Dates;
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
		public async Task<System.UInt64> SubmitSystemCommand(Sample.Core.Commands.MyOwnNameSpace.ISystemCommand systemCommand) {
			string path = $"{ControllerPath}/system";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Sample.Core.Commands.MyOwnNameSpace.ISystemCommand>(HttpMethod.Post, path, queryString, systemCommand)) {
				return await this.GetRequiredJsonResponseForValueType<System.UInt64>(request);
			}
		}

		public async Task<System.UInt64> SubmitAppCommand(Sample.Core.Commands.MyOwnNameSpace.IApplicationCommand applicationCommand) {
			string path = $"{ControllerPath}/app";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Sample.Core.Commands.MyOwnNameSpace.IApplicationCommand>(HttpMethod.Post, path, queryString, applicationCommand)) {
				return await this.GetRequiredJsonResponseForValueType<System.UInt64>(request);
			}
		}

		public async Task<System.UInt64> CommandSerializationErrorTest(System.Boolean callback) {
			string path = $"{ControllerPath}/command-serialization-error-test";
			var queryString = new NameValueCollection();
			queryString.Add("callback", $"{callback}");
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.UInt64>(request);
			}
		}
	}
}
#nullable disable

