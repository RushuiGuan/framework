using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class RequiredReturnTypeTestProxyService : ClientBase {
		public RequiredReturnTypeTestProxyService(ILogger<RequiredReturnTypeTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/required-return-type";
		public Task Get() {
			string path = $"{ControllerPath}/void";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task GetAsync() {
			string path = $"{ControllerPath}/async-task";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task GetActionResult() {
			string path = $"{ControllerPath}/action-result";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task GetAsyncActionResult() {
			string path = $"{ControllerPath}/async-action-result";
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

		public Task<System.Int32> GetInt() {
			string path = $"{ControllerPath}/int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public Task<System.Int32> GetAsyncInt() {
			string path = $"{ControllerPath}/async-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public Task<System.Int32> GetActionResultInt() {
			string path = $"{ControllerPath}/action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public Task<System.Int32> GetAsyncActionResultInt() {
			string path = $"{ControllerPath}/async-action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public Task<System.DateTime> GetDateTime() {
			string path = $"{ControllerPath}/datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public Task<System.DateTime> GetAsyncDateTime() {
			string path = $"{ControllerPath}/async-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public Task<System.DateTime> GetActionResultDateTime() {
			string path = $"{ControllerPath}/action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public Task<System.DateTime> GetAsyncActionResultDateTime() {
			string path = $"{ControllerPath}/async-action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public Task<System.DateOnly> GetDateOnly() {
			string path = $"{ControllerPath}/dateonly";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.DateOnly>(request);
			}
		}

		public Task<System.DateTimeOffset> GetDateTimeOffset() {
			string path = $"{ControllerPath}/datetimeoffset";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.DateTimeOffset>(request);
			}
		}

		public Task<System.TimeOnly> GetTimeOnly() {
			string path = $"{ControllerPath}/timeonly";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.TimeOnly>(request);
			}
		}

		public Task<Test.Dto.MyDto> GetMyDto() {
			string path = $"{ControllerPath}/object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto> GetAsyncMyDto() {
			string path = $"{ControllerPath}/async-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto> ActionResultObject() {
			string path = $"{ControllerPath}/action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto> AsyncActionResultObject() {
			string path = $"{ControllerPath}/async-action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}

		public Task<Test.Dto.MyDto[]> GetMyDtoArray() {
			string path = $"{ControllerPath}/array-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto[]>(request);
			}
		}

		public Task<System.Collections.Generic.IEnumerable<Test.Dto.MyDto>> GetMyDtoCollection() {
			string path = $"{ControllerPath}/collection-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<System.Collections.Generic.IEnumerable<Test.Dto.MyDto>>(request);
			}
		}
	}
}
#nullable disable

