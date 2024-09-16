using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class ReturnTypeTestProxyService : ClientBase {
		public ReturnTypeTestProxyService(ILogger<ReturnTypeTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/return-type-test";
		public Task Get() {
			string path = $"{ControllerPath}/void";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> GetString() {
			string path = $"{ControllerPath}/string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<Test.Dto.MyDto> GetMyDto() {
			string path = $"{ControllerPath}/my-dto";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task GetAsync() {
			string path = $"{ControllerPath}/async";
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

		public Task<System.String> GetAsyncNullableString() {
			string path = $"{ControllerPath}/async-nullable-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.Nullable<System.Int32>> GetAsyncNullableInt() {
			string path = $"{ControllerPath}/async-nullable-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public Task<Test.Dto.MyDto> GetAsyncMyDto() {
			string path = $"{ControllerPath}/async-my-dto";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto?> GetAsyncMyDtoNullable() {
			string path = $"{ControllerPath}/async-my-dto-nullable";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto> ActionResult() {
			string path = $"{ControllerPath}/action-result-generic";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto> AsyncActionResult() {
			string path = $"{ControllerPath}/async-action-result-generic";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task ActionResultAny() {
			string path = $"{ControllerPath}/action-result-any";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task AsyncActionResultAny() {
			string path = $"{ControllerPath}/async-action-result-any";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

