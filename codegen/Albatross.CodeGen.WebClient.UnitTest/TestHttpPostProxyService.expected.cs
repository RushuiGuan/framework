using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Albatross.Serialization;

#nullable enable
namespace Albatross.CodeGen.Tests.WebClient {
	public partial class TestHttpPostProxyService : Albatross.WebClient.ClientBase {
		public TestHttpPostProxyService(Microsoft.Extensions.Logging.ILogger @logger, System.Net.Http.HttpClient @client) : base(@logger, @client, Albatross.Serialization.DefaultJsonSettings.Value) {
		}
		public const System.String ControllerPath = "api/test-post";
		public async System.Threading.Tasks.Task<Albatross.CodeGen.Tests.Dto.MyDto> FromBody(Albatross.CodeGen.Tests.Dto.MyDto @dto) {
			string path = $"{ControllerPath}/from-body";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<Albatross.CodeGen.Tests.Dto.MyDto>(HttpMethod.Post, path, queryString, @dto)) {
				return await this.GetRequiredJsonResponse<Albatross.CodeGen.Tests.Dto.MyDto>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> PostStringOnly(System.String @body) {
			string path = $"{ControllerPath}/post-string";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, @body)) {
				return await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> QueryString(System.String @name) {
			string path = $"{ControllerPath}/query-string";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("name", @name);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> NamedQueryString(System.String @name) {
			string path = $"{ControllerPath}/named-query-string";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("n", @name);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> RouteParam(System.String @name) {
			string path = $"{ControllerPath}/route-param/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> Mixed(System.String @name, System.Int32 @id, Albatross.CodeGen.Tests.Dto.MyDto @dto) {
			string path = $"{ControllerPath}/mixed/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("id", System.Convert.ToString(@id));
			using (var request = this.CreateJsonRequest<Albatross.CodeGen.Tests.Dto.MyDto>(HttpMethod.Post, path, queryString, @dto)) {
				return await this.GetRawResponse(request);
			}
		}
		public async void AsyncVoid(System.Int32 @i) {
			string path = $"{ControllerPath}/async-void";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("i", System.Convert.ToString(@i));
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable
