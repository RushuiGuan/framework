using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface IInterfaceAndAbstractClassTestProxyService {
		Task SubmitByInterface(Test.Dto.Classes.ICommand command);
		Task SubmitByAbstractClass(Test.Dto.Classes.AbstractClass command);
		Task<Test.Dto.Classes.ICommand> ReturnInterfaceAsync();
		Task<Test.Dto.Classes.ICommand> ReturnInterface();
		Task<Test.Dto.Classes.AbstractClass> ReturnAbstractClassAsync();
		Task<Test.Dto.Classes.AbstractClass> ReturnAbstractClass();
	}

	public partial class InterfaceAndAbstractClassTestProxyService : ClientBase, IInterfaceAndAbstractClassTestProxyService {
		public InterfaceAndAbstractClassTestProxyService(ILogger<InterfaceAndAbstractClassTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/interface-abstract-class-test";
		public async Task SubmitByInterface(Test.Dto.Classes.ICommand command) {
			string path = $"{ControllerPath}/interface-as-param";
			var queryString = new NameValueCollection();
			queryString.Add("command", $"{command}");
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task SubmitByAbstractClass(Test.Dto.Classes.AbstractClass command) {
			string path = $"{ControllerPath}/abstract-class-as-param";
			var queryString = new NameValueCollection();
			queryString.Add("command", $"{command}");
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task<Test.Dto.Classes.ICommand> ReturnInterfaceAsync() {
			string path = $"{ControllerPath}/return-interface-async";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.ICommand>(request);
			}
		}

		public async Task<Test.Dto.Classes.ICommand> ReturnInterface() {
			string path = $"{ControllerPath}/return-interface";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.ICommand>(request);
			}
		}

		public async Task<Test.Dto.Classes.AbstractClass> ReturnAbstractClassAsync() {
			string path = $"{ControllerPath}/return-abstract-class-async";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.AbstractClass>(request);
			}
		}

		public async Task<Test.Dto.Classes.AbstractClass> ReturnAbstractClass() {
			string path = $"{ControllerPath}/return-abstract-class";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return await this.GetRequiredJsonResponse<Test.Dto.Classes.AbstractClass>(request);
			}
		}
	}
}
#nullable disable

