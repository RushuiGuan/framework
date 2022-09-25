using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.WebClient {
	public partial class TestHttpDeleteProxyService : Albatross.WebClient.ClientBase {
		public TestHttpDeleteProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client) : base(@logger, @client) {
		}
		public const System.String ControllerPath = "api/test-delete";
		public async void RouteOnly(System.String @name, System.Int32 @id) {
			string path = $"{ControllerPath}/route-only/{name}/{id}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task RouteWithDate(System.DateTime @date, System.Int32 @id) {
			string path = $"{ControllerPath}/route-with-date/{date:yyyy-MM-dd}/{id}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async void QueryStringOnly(System.String @name, System.Int32 @id) {
			string path = $"{ControllerPath}/query-string-only";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("name", @name);
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async void QueryStringWithDate(System.DateTime @date, System.Int32 @id) {
			string path = $"{ControllerPath}/query-string-with-date";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("date", string.Format("{0:yyyy-MM-dd}", @date));
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async void Mixed(System.String @name, System.Int32 @id) {
			string path = $"{ControllerPath}/mixed/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async void MixedDates(System.DateTime @tradeDate, System.DateTime @settlementDate) {
			string path = $"{ControllerPath}/mixed-dates/{tradeDate:yyyy-MM-dd}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("settlementDate", string.Format("{0:yyyy-MM-dd}", @settlementDate));
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task TestArrayInput(System.String[] @items) {
			string path = $"{ControllerPath}/array-query-string";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			foreach (var item in items) {
				queryString.Add("items", @item);
			}
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task TestWildCardRoute(System.String @name) {
			string path = $"{ControllerPath}/wild-card-route-param/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}