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
		public async System.Threading.Tasks.Task<System.Int32> LongRunningCommand(System.Int32 @counter, System.Int32 @duration) {
			string path = $"{ControllerPath}/long-running";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			queryString.Add("duration", System.Convert.ToString(@duration));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetJsonResponse<System.Int32>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.Int64> DoMathWork(System.Int64 @counter) {
			string path = $"{ControllerPath}/math-work";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetJsonResponse<System.Int64>(request);
			}
		}
		public async System.Threading.Tasks.Task DoFireAndForgetMathWork(System.Int64 @counter) {
			string path = $"{ControllerPath}/fire-and-forget-math-work";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task<System.Int64> ProcessData(System.Int64 @counter) {
			string path = $"{ControllerPath}/process-data";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetJsonResponse<System.Int64>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.Int32> Unstable(System.Int32 @counter) {
			string path = $"{ControllerPath}/unstable";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetJsonResponse<System.Int32>(request);
			}
		}
		public async System.Threading.Tasks.Task FireAndForget(System.Int32 @counter, System.Nullable<System.Int32> @duration) {
			string path = $"{ControllerPath}/fire-and-forget";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			queryString.Add("duration", System.Convert.ToString(@duration));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task FireAndWait(System.Int32 @counter, System.Nullable<System.Int32> @duration) {
			string path = $"{ControllerPath}/fire-and-wait";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("counter", System.Convert.ToString(@counter));
			queryString.Add("duration", System.Convert.ToString(@duration));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
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
		public async System.Threading.Tasks.Task Publish(System.String @topic, System.Int32 @min, System.Int32 @max) {
			string path = $"{ControllerPath}/pub";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("topic", @topic);
			queryString.Add("min", System.Convert.ToString(@min));
			queryString.Add("max", System.Convert.ToString(@max));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
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
		public async System.Threading.Tasks.Task Ping(System.Int32 @round) {
			string path = $"{ControllerPath}/play-ping";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("round", System.Convert.ToString(@round));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task Pong(System.Int32 @round) {
			string path = $"{ControllerPath}/play-pong";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("round", System.Convert.ToString(@round));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
