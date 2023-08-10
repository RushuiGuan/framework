using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using System.IO;
using Albatross.Serialization;

namespace Albatross.WebClient.Test {
	public partial class ValueProxyService : Albatross.WebClient.ClientBase {
		public ValueProxyService(Microsoft.Extensions.Logging.ILogger<ValueProxyService> @logger, System.Net.Http.HttpClient @client, IJsonSettings serializationOption) : base(@logger, @client, serializationOption) {
		}
		public const System.String ControllerPath = "/api/value";
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

		public async System.Threading.Tasks.Task<System.String> GetConfig() {
			string path = $"{ControllerPath}/config";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.String>(request);
			}
		}

		public async System.Threading.Tasks.Task<System.String> Timeout(int seconds) {
			string path = $"{ControllerPath}/timeout";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("seconds", seconds.ToString());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
