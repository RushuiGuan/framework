using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface IArrayParamTestProxyService {
		Task<System.String> ArrayStringParam(System.String[] array);
		Task<System.String> ArrayValueType(System.Int32[] array);
		Task<System.String> CollectionStringParam(System.Collections.Generic.IEnumerable<System.String> collection);
		Task<System.String> CollectionValueType(System.Collections.Generic.IEnumerable<System.Int32> collection);
	}

	public partial class ArrayParamTestProxyService : ClientBase, IArrayParamTestProxyService {
		public ArrayParamTestProxyService(ILogger<ArrayParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/array-param-test";
		public async Task<System.String> ArrayStringParam(System.String[] array) {
			string path = $"{ControllerPath}/array-string-param";
			var queryString = new NameValueCollection();
			foreach (var item in array) {
				queryString.Add("a", item);
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> ArrayValueType(System.Int32[] array) {
			string path = $"{ControllerPath}/array-value-type";
			var queryString = new NameValueCollection();
			foreach (var item in array) {
				queryString.Add("a", $"{item}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> CollectionStringParam(System.Collections.Generic.IEnumerable<System.String> collection) {
			string path = $"{ControllerPath}/collection-string-param";
			var queryString = new NameValueCollection();
			foreach (var item in collection) {
				queryString.Add("c", item);
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> CollectionValueType(System.Collections.Generic.IEnumerable<System.Int32> collection) {
			string path = $"{ControllerPath}/collection-value-type";
			var queryString = new NameValueCollection();
			foreach (var item in collection) {
				queryString.Add("c", $"{item}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

