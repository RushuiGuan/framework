using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;

namespace Albatross.IAM.WebClient {
	public partial class GroupClientService : Albatross.WebClient.ClientBase {
		public GroupClientService(Microsoft.Extensions.Logging.ILogger<GroupClientService> @logger, System.Net.Http.HttpClient @client, Microsoft.Extensions.Configuration.IConfiguration @config) : base(@logger, @client, @config) {
		}
		public const System.String ControllerPath = "api/group";
		public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Albatross.IAM.Messages.PrincipalDto>> GetNormal() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<System.Collections.Generic.IEnumerable<Albatross.IAM.Messages.PrincipalDto>>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Albatross.IAM.Messages.PrincipalDto>> GetNormalAsync() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<System.Collections.Generic.IEnumerable<Albatross.IAM.Messages.PrincipalDto>>(request);
			}
		}
		public async System.Threading.Tasks.Task GetVoid() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task GetTask() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.Invoke(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> GetString() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> GetStringAsync() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.Invoke<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> RouteOnly(System.Int32 @id, System.String @name) {
			string path = $"{ControllerPath}/{id}/{name}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.Invoke<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> QueryStrings(System.Int32 @id, System.String @name) {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add(nameof(@id), System.Convert.ToString(@id));
			queryString.Add(nameof(@name), @name);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.Invoke<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> JsonObject(Albatross.IAM.Api.NameDto @dto) {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<Albatross.IAM.Api.NameDto>(HttpMethod.Post, path, queryString, @dto)) {
				return await this.Invoke<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> JsonObject(Albatross.IAM.Api.NameDto @dto1, Albatross.IAM.Api.NameDto @dto2) {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<Albatross.IAM.Api.NameDto>(HttpMethod.Post, path, queryString, @dto2)) {
				return await this.Invoke<System.String>(request);
			}
		}
		public async System.Threading.Tasks.Task<System.String> MixedRouteQueryStringAndJson(System.Int32 @groupID, System.Int32 @userID, System.String @name, System.String @criteria, Albatross.IAM.Api.NameDto @dto) {
			string path = $"{ControllerPath}/{groupID}/{userID}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add(nameof(@name), @name);
			queryString.Add(nameof(@criteria), @criteria);
			using (var request = this.CreateJsonRequest<Albatross.IAM.Api.NameDto>(HttpMethod.Post, path, queryString, @dto)) {
				return await this.Invoke<System.String>(request);
			}
		}
	}
}