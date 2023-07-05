using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Albatross.Serialization;

namespace SampleProject.Proxy {
	public partial class RunProxyService : Albatross.WebClient.ClientBase {
		public RunProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client, Albatross.Serialization.IJsonSerializationOption @serializationOptions) : base(@logger, @client, @serializationOptions) {
		}
		public const System.String ControllerPath = "api/run";
		public async System.Threading.Tasks.Task Ping() {
			string path = $"{ControllerPath}/ping";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> QueueStatus() {
			string path = $"{ControllerPath}/queue-status";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task Subscribe(System.String @topic) {
			string path = $"{ControllerPath}/sub";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("topic", @topic);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task Unsubscribe(System.String @topic) {
			string path = $"{ControllerPath}/unsub";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("topic", @topic);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task UnsubscribeAll() {
			string path = $"{ControllerPath}/unsub-all";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
