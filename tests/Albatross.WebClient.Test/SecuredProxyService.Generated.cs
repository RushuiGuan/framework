using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Albatross.Serialization;

namespace Albatross.WebClient.Test {
	public partial class SecuredProxyService : Albatross.WebClient.ClientBase {
		public SecuredProxyService(Microsoft.Extensions.Logging.ILogger<SecuredProxyService> @logger, System.Net.Http.HttpClient @client, IJsonSettings serializationOption) : base(@logger, @client, serializationOption) {
		}
		public const System.String ControllerPath = "/api/secured";
		public async System.Threading.Tasks.Task<PayLoad> GetJson() {
			string path = $"{ControllerPath}/json";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<PayLoad>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> GetText() {
			string path = $"{ControllerPath}/text";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<PayLoad> Post(PayLoad @payLoad) {
			string path = $"{ControllerPath}/post";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<PayLoad>(HttpMethod.Post, path, queryString, @payLoad)) {
				return await this.GetJsonResponse<PayLoad>(request);
			}
		}
	}
}
