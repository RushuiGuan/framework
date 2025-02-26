using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface INullableReturnTypeTestProxyService {
		Task<System.String> GetString();
		Task<System.String> GetAsyncString();
		Task<System.String> GetActionResultString();
		Task<System.String> GetAsyncActionResultString();
		Task<System.Nullable<System.Int32>> GetInt();
		Task<System.Nullable<System.Int32>> GetAsyncInt();
		Task<System.Nullable<System.Int32>> GetActionResultInt();
		Task<System.Nullable<System.Int32>> GetAsyncActionResultInt();
		Task<System.Nullable<System.DateTime>> GetDateTime();
		Task<System.Nullable<System.DateTime>> GetAsyncDateTime();
		Task<System.Nullable<System.DateTime>> GetActionResultDateTime();
		Task<System.Nullable<System.DateTime>> GetAsyncActionResultDateTime();
		Task<Test.Dto.Classes.MyDto?> GetMyDto();
		Task<Test.Dto.Classes.MyDto?> GetAsyncMyDto();
		Task<Test.Dto.Classes.MyDto?> ActionResultObject();
		Task<Test.Dto.Classes.MyDto?> AsyncActionResultObject();
		Task<Test.Dto.Classes.MyDto?[]> GetMyDtoNullableArray();
		Task<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>> GetMyDtoCollection();
	}

	public partial class NullableReturnTypeTestProxyService : ClientBase, INullableReturnTypeTestProxyService {
		public NullableReturnTypeTestProxyService(ILogger<NullableReturnTypeTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/nullable-return-type";
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

		public async Task<System.Nullable<System.Int32>> GetInt() {
			string path = $"{ControllerPath}/int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public async Task<System.Nullable<System.Int32>> GetAsyncInt() {
			string path = $"{ControllerPath}/async-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public async Task<System.Nullable<System.Int32>> GetActionResultInt() {
			string path = $"{ControllerPath}/action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public async Task<System.Nullable<System.Int32>> GetAsyncActionResultInt() {
			string path = $"{ControllerPath}/async-action-result-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.Int32>>(request);
			}
		}

		public async Task<System.Nullable<System.DateTime>> GetDateTime() {
			string path = $"{ControllerPath}/datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public async Task<System.Nullable<System.DateTime>> GetAsyncDateTime() {
			string path = $"{ControllerPath}/async-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public async Task<System.Nullable<System.DateTime>> GetActionResultDateTime() {
			string path = $"{ControllerPath}/action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public async Task<System.Nullable<System.DateTime>> GetAsyncActionResultDateTime() {
			string path = $"{ControllerPath}/async-action-result-datetime";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<System.Nullable<System.DateTime>>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto?> GetMyDto() {
			string path = $"{ControllerPath}/object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<Test.Dto.Classes.MyDto?>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto?> GetAsyncMyDto() {
			string path = $"{ControllerPath}/async-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<Test.Dto.Classes.MyDto?>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto?> ActionResultObject() {
			string path = $"{ControllerPath}/action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<Test.Dto.Classes.MyDto?>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto?> AsyncActionResultObject() {
			string path = $"{ControllerPath}/async-action-result-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<Test.Dto.Classes.MyDto?>(request);
			}
		}

		public async Task<Test.Dto.Classes.MyDto?[]> GetMyDtoNullableArray() {
			string path = $"{ControllerPath}/nullable-array-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.MyDto?[]>(request);
			}
		}

		public async Task<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>> GetMyDtoCollection() {
			string path = $"{ControllerPath}/nullable-collection-return-type";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponse<System.Collections.Generic.IEnumerable<Test.Dto.Classes.MyDto>>(request);
			}
		}
	}
}
#nullable disable

