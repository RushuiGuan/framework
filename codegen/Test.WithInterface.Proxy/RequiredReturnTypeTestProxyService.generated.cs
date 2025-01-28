using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface IRequiredReturnTypeTestProxyService {
		Task Get();
		Task GetAsync();
		Task GetActionResult();
		Task GetAsyncActionResult();
		Task<System.String> GetString();
		Task<System.String> GetAsyncString();
		Task<System.String> GetActionResultString();
		Task<System.String> GetAsyncActionResultString();
		Task<System.Int32> GetInt();
		Task<System.Int32> GetAsyncInt();
		Task<System.Int32> GetActionResultInt();
		Task<System.Int32> GetAsyncActionResultInt();
		Task<System.DateTime> GetDateTime();
		Task<System.DateTime> GetAsyncDateTime();
		Task<System.DateTime> GetActionResultDateTime();
		Task<System.DateTime> GetAsyncActionResultDateTime();
		Task<System.DateOnly> GetDateOnly();
		Task<System.DateTimeOffset> GetDateTimeOffset();
		Task<System.TimeOnly> GetTimeOnly();
		Task<Test.Dto.Classes.MyDto> GetMyDto();
		Task<Test.Dto.Classes.MyDto> GetAsyncMyDto();
		Task<Test.Dto.Classes.MyDto> ActionResultObject();
		Task<Test.Dto.Classes.MyDto> AsyncActionResultObject();
		Task<Test.Dto.Classes.MyDto[]> GetMyDtoArray();
		Task<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>> GetMyDtoCollection();
		Task<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>> GetMyDtoCollectionAsync();
	}

	public partial class RequiredReturnTypeTestProxyService : ClientBase, IRequiredReturnTypeTestProxyService {
		public RequiredReturnTypeTestProxyService(ILogger<RequiredReturnTypeTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/required-return-type";
		public async Task Get() {
			string path = $"{ControllerPath}/void";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task GetAsync() {
			string path = $"{ControllerPath}/async-task";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task GetActionResult() {
			string path = $"{ControllerPath}/action-result";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task GetAsyncActionResult() {
			string path = $"{ControllerPath}/async-action-result";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> GetString() {
			string path = $"{ControllerPath}/string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> GetAsyncString() {
			string path = $"{ControllerPath}/async-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> GetActionResultString() {
			string path = $"{ControllerPath}/action-result-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> GetAsyncActionResultString() {
			string path = $"{ControllerPath}/async-action-result-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.Int32> GetInt() {
			string path = $"{ControllerPath}/int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public async Task<System.Int32> GetAsyncInt() {
			string path = $"{ControllerPath}/async-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public async Task<System.Int32> GetActionResultInt() {
			string path = $"{ControllerPath}/action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public async Task<System.Int32> GetAsyncActionResultInt() {
			string path = $"{ControllerPath}/async-action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public async Task<System.DateTime> GetDateTime() {
			string path = $"{ControllerPath}/datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public async Task<System.DateTime> GetAsyncDateTime() {
			string path = $"{ControllerPath}/async-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public async Task<System.DateTime> GetActionResultDateTime() {
			string path = $"{ControllerPath}/action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public async Task<System.DateTime> GetAsyncActionResultDateTime() {
			string path = $"{ControllerPath}/async-action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.DateTime>(request);
			}
		}

		public async Task<System.DateOnly> GetDateOnly() {
			string path = $"{ControllerPath}/dateonly";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.DateOnly>(request);
			}
		}

		public async Task<System.DateTimeOffset> GetDateTimeOffset() {
			string path = $"{ControllerPath}/datetimeoffset";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.DateTimeOffset>(request);
			}
		}

		public async Task<System.TimeOnly> GetTimeOnly() {
			string path = $"{ControllerPath}/timeonly";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.TimeOnly>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto> GetMyDto() {
			string path = $"{ControllerPath}/object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.MyDto>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto> GetAsyncMyDto() {
			string path = $"{ControllerPath}/async-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.MyDto>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto> ActionResultObject() {
			string path = $"{ControllerPath}/action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.MyDto>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto> AsyncActionResultObject() {
			string path = $"{ControllerPath}/async-action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.MyDto>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto[]> GetMyDtoArray() {
			string path = $"{ControllerPath}/array-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.MyDto[]>(request);
			}
		}

		public async Task<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>> GetMyDtoCollection() {
			string path = $"{ControllerPath}/collection-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>>(request);
			}
		}

		public async Task<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>> GetMyDtoCollectionAsync() {
			string path = $"{ControllerPath}/async-collection-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>>(request);
			}
		}
	}
}
#nullable disable

