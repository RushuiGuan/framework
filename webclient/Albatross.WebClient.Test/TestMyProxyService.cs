using Albatross.Serialization;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.WebClient.Test {
	public class MyProxyService : WebClient.ClientBase {
		public MyProxyService(ILogger logger, HttpClient client, IJsonSettings serializationOption) : base(logger, client, serializationOption) {
		}
		public async Task<string> RunMe() {
			using var request = CreateRequest(HttpMethod.Get, string.Empty, new NameValueCollection());
			return await this.GetRawResponse(request);
		}
	}
	public class TestMyProxyService {
		[Fact]
		public void TestRequestGeneration1() {
			List<int> ids = new List<int>();
			for (int i = 0; i < 1000; i++) {
				ids.Add(i);
			}
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://myyyhost/mmmyyy-data");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client, new DefaultJsonSettings());
			string path = $"api/w9-bar";
			var queryString = new NameValueCollection();
			queryString.Add("date", string.Format("{0:yyyy-MM-dd}", DateTime.Today));
			queryString.Add("isEquity", Convert.ToString(false));
			var arrayQueryString = new List<string>();
			foreach (var item in ids) {
				arrayQueryString.Add(Convert.ToString(@item));
			}
			var requests = proxy.CreateRequestUrls(path, queryString, 2000, "id", arrayQueryString.ToArray());
			Assert.Equal(4, requests.Count());
		}

		[Fact]
		public void TestRequestGeneration2() {
			List<int> ids = new List<int>();
			for (int i = 0; i < 50; i++) {
				ids.Add(i);
			}
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://myyyhost/mmmyyy-data");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client, new DefaultJsonSettings());
			string path = $"api/w9-bar";
			var queryString = new NameValueCollection();
			queryString.Add("date", string.Format("{0:yyyy-MM-dd}", DateTime.Today));
			queryString.Add("isEquity", Convert.ToString(false));
			var arrayQueryString = new List<string>();
			foreach (var item in ids) {
				arrayQueryString.Add(Convert.ToString(@item));
			}
			var requests = proxy.CreateRequestUrls(path, queryString, 2000, "id", arrayQueryString.ToArray());
			Assert.Single(requests);
		}
		[InlineData(45, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		[InlineData(46, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		[InlineData(47, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		[InlineData(48, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		[InlineData(49, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		[InlineData(50, 2, "api/bar?date=2022-10-10&id=0&id=1&")]
		[InlineData(51, 2, "api/bar?date=2022-10-10&id=0&id=1&")]
		[InlineData(52, 2, "api/bar?date=2022-10-10&id=0&id=1&")]
		[Theory]
		public void TestRequestGeneration3(int maxlength, int count, params string[] expected) {
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://myyyhost");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client, new DefaultJsonSettings());
			string path = $"api/bar";
			var queryString = new NameValueCollection();
			queryString.Add("date", "2022-10-10");
			var arrayQueryString = new List<string>();
			for (int i = 0; i < count; i++) {
				arrayQueryString.Add(i.ToString());
			}
			var requests = proxy.CreateRequestUrls(path, queryString, maxlength, "id", arrayQueryString.ToArray()).ToArray();
			Assert.Equal(expected.Length, requests.Count());
			List<(string expectedUrl, string actualUrl)> list = new List<(string expectedUrl, string actualUrl)>();
			List<Action<(string expectedUrl, string actualUrl)>> actions = new List<Action<(string, string)>>();
			for (int i = 0; i < expected.Length; i++) {
				list.Add((expected[i], requests[i]));
				actions.Add(args => Assert.Equal(args.expectedUrl, args.actualUrl));
			}
			Assert.Collection(list, actions.ToArray());
		}

		[Theory]
		[InlineData(2, 2)]
		[InlineData(44, 2)]
		public void TestInsufficientMaxLength(int maxlength, int count) {
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://myyyhost");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client, new DefaultJsonSettings());
			string path = $"api/bar";
			var queryString = new NameValueCollection {
				{ "date", "2022-10-10" }
			};
			var arrayQueryString = new List<string>();
			for (int i = 0; i < count; i++) {
				arrayQueryString.Add(i.ToString());
			}
			Assert.Throws<InvalidOperationException>(() => proxy.CreateRequestUrls(path, queryString, maxlength, "id", arrayQueryString.ToArray()));
		}
		/*
http://myyyhost/api/bar?date=2022-10-10&id=0%2C1
http://myyyhost/api/bar?date=2022-10-10&id=0
		 */
		//[InlineData(45, 2,".", "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		//[InlineData(46, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		//[InlineData(47, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		//[InlineData(48, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		//[InlineData(49, 2, "api/bar?date=2022-10-10&id=0&", "api/bar?date=2022-10-10&id=1&")]
		[InlineData(50, 2, ".", "api/bar?date=2022-10-10&id=0.1")]
		[InlineData(51, 2, ".", "api/bar?date=2022-10-10&id=0.1")]
		[InlineData(52, 2, ".", "api/bar?date=2022-10-10&id=0.1")]
		[InlineData(52, 2, ",", "api/bar?date=2022-10-10&id=0%2C1")]
		[InlineData(48, 2, ",", "api/bar?date=2022-10-10&id=0%2C1")]
		[InlineData(47, 2, ",", "api/bar?date=2022-10-10&id=0", "api/bar?date=2022-10-10&id=1")]
		[InlineData(44, 2, ",", "api/bar?date=2022-10-10&id=0", "api/bar?date=2022-10-10&id=1")]
		[Theory]
		public void TestDelimitedRequestGeneration(int maxlength, int count, string delimiter, params string[] expected) {
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://myyyhost");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client, new DefaultJsonSettings());
			string path = $"api/bar";
			var queryString = new NameValueCollection {
				{ "date", "2022-10-10" }
			};
			var arrayQueryString = new List<string>();
			for (int i = 0; i < count; i++) {
				arrayQueryString.Add(i.ToString());
			}
			var requests = proxy.CreateRequestUrlsByDelimitedQueryString(path, queryString, maxlength, "id", delimiter, arrayQueryString.ToArray()).ToArray();
			Assert.Equal(expected.Length, requests.Count());
			List<(string expectedUrl, string actualUrl)> list = new List<(string expectedUrl, string actualUrl)>();
			List<Action<(string expectedUrl, string actualUrl)>> actions = new List<Action<(string, string)>>();
			for (int i = 0; i < expected.Length; i++) {
				list.Add((expected[i], requests[i]));
				actions.Add(args => Assert.Equal(args.expectedUrl, args.actualUrl));
			}
			Assert.Collection(list, actions.ToArray());
		}

		[Theory]
		[InlineData(2, 2, ",")]
		[InlineData(43, 2, ",")]
		public void TestInsufficientMaxLengthWithDelimiter(int maxlength, int count, string delimiter) {
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://myyyhost");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client, new DefaultJsonSettings());
			string path = $"api/bar";
			var queryString = new NameValueCollection();
			queryString.Add("date", "2022-10-10");
			var arrayQueryString = new List<string>();
			for (int i = 0; i < count; i++) {
				arrayQueryString.Add(i.ToString());
			}
			Assert.Throws<InvalidOperationException>(() => proxy.CreateRequestUrlsByDelimitedQueryString(path, queryString, maxlength, "id", delimiter, arrayQueryString.ToArray()));
		}
	}
}