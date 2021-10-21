using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.WebClient {
	public partial class ValueProxyService : Albatross.WebClient.ClientBase {
		public ValueProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client) : base(@logger, @client) {
		}
		public const System.String ControllerPath = "api/value";
		public async System.Threading.Tasks.Task<Albatross.WebClient.IntegrationTest.Messages.PayLoad> GetJson() {
			string path = $"{ControllerPath}/json";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.IntegrationTest.Messages.PayLoad>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> GetText() {
			string path = $"{ControllerPath}/text";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.IntegrationTest.Messages.PayLoad> Post(Albatross.WebClient.IntegrationTest.Messages.PayLoad @payLoad) {
			string path = $"{ControllerPath}/post";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<Albatross.WebClient.IntegrationTest.Messages.PayLoad>(HttpMethod.Post, path, queryString, @payLoad)) {
				return await this.Invoke<Albatross.WebClient.IntegrationTest.Messages.PayLoad>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> TestArrayInput(System.String[] @items) {
			string path = $"{ControllerPath}/array-input";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			foreach (var item in items) {
				queryString.Add("items", @item);
			}
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke(request);
			}
		}
	}
}