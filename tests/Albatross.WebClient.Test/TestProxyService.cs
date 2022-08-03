using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Albatross.WebClient.Test{
	public class MyProxyService : WebClient.ClientBase {
		public MyProxyService(ILogger logger, HttpClient client) : base(logger, client) {
		}
	}
	public class TestProxyService {
		[Fact]
		public void TestRequestGeneration1() {
			List<int> ids = new List<int>();
			for (int i = 0; i < 1000; i++) {
				ids.Add(i);
			}
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://app-prod/market-data");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client);
			string path = $"api/w9-bar";
			var queryString = new NameValueCollection();
			queryString.Add("date", string.Format("{0:yyyy-MM-dd}", DateTime.Today));
			queryString.Add("isEquity", Convert.ToString(false));
			var arrayQueryString = new List<string>();
			foreach (var item in ids) {
				arrayQueryString.Add(Convert.ToString(@item));
			}
			var requests = proxy.CreateRequests(HttpMethod.Get, path, queryString, 2000, "id", arrayQueryString.ToArray());
			Assert.Equal(4, requests.Count());
		}

		[Fact]
		public void TestRequestGeneration2() {
			List<int> ids = new List<int>();
			for (int i = 0; i < 50; i++) {
				ids.Add(i);
			}
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://app-prod/market-data");
			var proxy = new MyProxyService(new Mock<ILogger>().Object, client);
			string path = $"api/w9-bar";
			var queryString = new NameValueCollection();
			queryString.Add("date", string.Format("{0:yyyy-MM-dd}", DateTime.Today));
			queryString.Add("isEquity", Convert.ToString(false));
			var arrayQueryString = new List<string>();
			foreach (var item in ids) {
				arrayQueryString.Add(Convert.ToString(@item));
			}
			var requests = proxy.CreateRequests(HttpMethod.Get, path, queryString, 2000, "id", arrayQueryString.ToArray());
			Assert.Single(requests);
		}
	}
}
