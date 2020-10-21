using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Xunit;
using System.Collections.Specialized;

namespace Albatross.WebClient.IntegrationTest {
	public partial class TestExtensionMethods {
		[Fact]
		public void TestGetUrlWithNullQueryString() {
			string url = "http://localhost";
			NameValueCollection values = new NameValueCollection();
			values.Add("name", null);
			string result = url.GetUrl(values);
			Assert.Equal("http://localhost?", result);
		}

		[Fact]
		public void TestGetUrlWithNullQueryString2() {
			string url = "http://localhost";
			NameValueCollection values = new NameValueCollection();
			values.Add("name", null);
			values.Add("value", "a");
			string result = url.GetUrl(values);
			Assert.Equal("http://localhost?value=a&", result);
		}
	}
}
