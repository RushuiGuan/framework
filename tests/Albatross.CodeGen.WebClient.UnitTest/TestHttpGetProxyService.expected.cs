using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.WebClient {
	public partial class TestHttpGetProxyService : Albatross.WebClient.ClientBase {
		public TestHttpGetProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client) : base(@logger, @client) {
		}
		public const System.String ControllerPath = "api/test-get";
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> GetObject() {
			string path = $"{ControllerPath}/object";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> GetObjectAsync() {
			string path = $"{ControllerPath}/object-async";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async void GetVoid() {
			string path = $"{ControllerPath}/void";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task GetTask() {
			string path = $"{ControllerPath}/task";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> GetString() {
			string path = $"{ControllerPath}/string";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> GetStringAsync() {
			string path = $"{ControllerPath}/string-async";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> RouteOnly(System.String @name, System.Int32 @id) {
			string path = $"{ControllerPath}/route-only/{name}/{id}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> RouteWithDate(System.DateTime @date, System.Int32 @id) {
			string path = $"{ControllerPath}/route-with-date/{date}/{id}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> QueryStringOnly(System.String @name, System.Int32 @id) {
			string path = $"{ControllerPath}/query-string-only";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("name", @name);
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> QueryStringWithDate(System.DateTime @date, System.Int32 @id) {
			string path = $"{ControllerPath}/query-string-with-date";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("date", string.Format("{0:yyyy-MM-dd}", @date));
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> Mixed(System.String @name, System.Int32 @id) {
			string path = $"{ControllerPath}/mixed/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<Albatross.WebClient.Test.Messages.Dto> MixedDates(System.DateTime @tradeDate, System.DateTime @settlementDate) {
			string path = $"{ControllerPath}/mixed-dates/{tradeDate}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("settlementDate", string.Format("{0:yyyy-MM-dd}", @settlementDate));
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<Albatross.WebClient.Test.Messages.Dto>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> TestArrayInput(System.String[] @items) {
			string path = $"{ControllerPath}/array-query-string";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			foreach (var item in items) {
				queryString.Add("items", @item);
			}
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> TestWildCardRoute(System.String @name) {
			string path = $"{ControllerPath}/wild-card-route-param/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke(request);
			}
		}
	}
}