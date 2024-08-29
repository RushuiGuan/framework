using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Albatross.Serialization;

#nullable enable
namespace Sample.Proxy {
	public partial class CommandProxyService : Albatross.WebClient.ClientBase {
		public CommandProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client) : base(@logger, @client, Albatross.Serialization.DefaultJsonSettings.Value) {
		}
		public const System.String ControllerPath = "api/command";
		public async System.Threading.Tasks.Task SubmitSystemCommand(Sample.Core.Commands.MyOwnNameSpace.ISystemCommand @systemCommand) {
			string path = $"{ControllerPath}/system";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("systemCommand", System.Convert.ToString(@systemCommand));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task SubmitAppCommand(Sample.Core.Commands.MyOwnNameSpace.IApplicationCommand @applicationCommand) {
			string path = $"{ControllerPath}/app";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("applicationCommand", System.Convert.ToString(@applicationCommand));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

