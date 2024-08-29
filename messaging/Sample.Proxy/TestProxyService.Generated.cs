using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Albatross.Serialization;

#nullable enable
namespace Sample.Proxy {
	public partial class TestProxyService : Albatross.WebClient.ClientBase {
		public TestProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client) : base(@logger, @client, Albatross.Serialization.DefaultJsonSettings.Value) {
		}
		public const System.String ControllerPath = "api/test";
		public async System.Threading.Tasks.Task<System.String> TestPostPlainTextToBody(System.String @text) {
			string path = $"{ControllerPath}/plain-text-post";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, @text)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

