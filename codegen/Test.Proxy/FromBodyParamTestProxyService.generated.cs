using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class FromBodyParamTestProxyService : ClientBase {
		public FromBodyParamTestProxyService(ILogger<FromBodyParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/from-body-param-test";
		public async Task RequiredObject(Test.Dto.Classes.MyDto dto) {
			string path = $"{ControllerPath}/ required-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.Classes.MyDto>(HttpMethod.Post, path, queryString, dto)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task NullableObject(Test.Dto.Classes.MyDto? dto) {
			string path = $"{ControllerPath}/ nullable-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.Classes.MyDto?>(HttpMethod.Post, path, queryString, dto)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredInt(System.Int32 value) {
			string path = $"{ControllerPath}/ required-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<System.Int32>(HttpMethod.Post, path, queryString, value)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task NullableInt(System.Nullable<System.Int32> value) {
			string path = $"{ControllerPath}/ nullable-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<System.Nullable<System.Int32>>(HttpMethod.Post, path, queryString, value)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredString(System.String value) {
			string path = $"{ControllerPath}/ required-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, value)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task NullableString(System.String? value) {
			string path = $"{ControllerPath}/ nullable-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, value)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredObjectArray(Test.Dto.Classes.MyDto[] array) {
			string path = $"{ControllerPath}/ required-object-array";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.Classes.MyDto[]>(HttpMethod.Post, path, queryString, array)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task NullableObjectArray(Test.Dto.Classes.MyDto?[] array) {
			string path = $"{ControllerPath}/ nullable-object-array";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.Classes.MyDto?[]>(HttpMethod.Post, path, queryString, array)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

