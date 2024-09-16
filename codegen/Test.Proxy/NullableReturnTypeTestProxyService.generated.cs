using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class NullableReturnTypeTestProxyService : ClientBase {
		public NullableReturnTypeTestProxyService(ILogger<NullableReturnTypeTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/nullable-return-type";
		public Task<System.String> GetString() {
			string path = $"{ControllerPath}/string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> GetAsyncString() {
			string path = $"{ControllerPath}/async-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> GetActionResultString() {
			string path = $"{ControllerPath}/action-result-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> GetAsyncActionResultString() {
			string path = $"{ControllerPath}/async-action-result-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.Nullable<System.Int32>> GetInt() {
			string path = $"{ControllerPath}/int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public Task<System.Nullable<System.Int32>> GetAsyncInt() {
			string path = $"{ControllerPath}/async-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public Task<System.Nullable<System.Int32>> GetActionResultInt() {
			string path = $"{ControllerPath}/action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public Task<System.Nullable<System.Int32>> GetAsyncActionResultInt() {
			string path = $"{ControllerPath}/async-action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public Task<System.Nullable<System.DateTime>> GetDateTime() {
			string path = $"{ControllerPath}/datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public Task<System.Nullable<System.DateTime>> GetAsyncDateTime() {
			string path = $"{ControllerPath}/async-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public Task<System.Nullable<System.DateTime>> GetActionResultDateTime() {
			string path = $"{ControllerPath}/action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public Task<System.Nullable<System.DateTime>> GetAsyncActionResultDateTime() {
			string path = $"{ControllerPath}/async-action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public Task<Test.Dto.MyDto?> GetMyDto() {
			string path = $"{ControllerPath}/object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<Test.Dto.MyDto?>(request);
			}
		}

		public Task<Test.Dto.MyDto?> GetAsyncMyDto() {
			string path = $"{ControllerPath}/async-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<Test.Dto.MyDto?>(request);
			}
		}

		public Task<Test.Dto.MyDto?> ActionResultObject() {
			string path = $"{ControllerPath}/action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<Test.Dto.MyDto?>(request);
			}
		}

		public Task<Test.Dto.MyDto?> AsyncActionResultObject() {
			string path = $"{ControllerPath}/async-action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<Test.Dto.MyDto?>(request);
			}
		}

		public Task<Test.Dto.MyDto?[]> GetMyDtoNullableArray() {
			string path = $"{ControllerPath}/nullable-array-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto?[]>(request);
			}
		}

		public Task<System.Collections.Generic.IEnumerable<Test.Dto.MyDto>> GetMyDtoCollection() {
			string path = $"{ControllerPath}/nullable-collection-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<System.Collections.Generic.IEnumerable<Test.Dto.MyDto>>(request);
			}
		}
	}
}
#nullable disable

