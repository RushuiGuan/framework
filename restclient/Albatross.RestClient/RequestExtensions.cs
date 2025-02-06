using Albatross.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.RestClient {
	public static class RequestExtensions {
		#region creating request
		public static StringBuilder CreateUrl(this string url, NameValueCollection queryStringValues) {
			var sb = new StringBuilder(url);
			if (queryStringValues?.Count > 0) {
				sb.Append("?");
				for (int i = 0; i < queryStringValues.Count; i++) {
					string[] values = queryStringValues.GetValues(i) ?? new string[0];
					string key = queryStringValues.GetKey(i) ?? string.Empty;
					foreach (string value in values) {
						sb.AddQueryParam(key, value);
					}
				}
			}
			return sb;
		}
		public static void AddQueryParam(this StringBuilder sb, string key, string value) {
			sb.Append(Uri.EscapeDataString(key));
			sb.Append("=");
			sb.Append(Uri.EscapeDataString(value));
			sb.Append("&");
		}
		public static void AddMultiPartFormData(this MultipartFormDataContent content, MultiPartFormData data) {
			HttpContent part;
			if (data.Stream != null) {
				data.Stream.Position = 0;
				part = new StreamContent(data.Stream);
			} else {
				part = new ByteArrayContent(data.Data);
			}
			if (!string.IsNullOrEmpty(data.ContentType)) {
				part.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(data.ContentType);
			}
			if (string.IsNullOrEmpty(data.Filename)) {
				content.Add(part, data.Name);
			} else {
				content.Add(part, data.Name, data.Filename);
			}
		}
		public static Uri RequiredBaseUrl(this HttpClient client) => client.BaseAddress ?? throw new InvalidOperationException("HttpClient BaseAddress is not set");
		public static IEnumerable<string> CreateRequestUrls(Uri baseUrl, string relativeUrl, NameValueCollection queryStrings, int maxUrlLength, string arrayQueryStringKey, params string[] arrayQueryStringValues) {
			var urls = new List<string>();
			int offset = 0;
			do {
				var sb = relativeUrl.CreateUrl(queryStrings);
				int index;
				for (index = offset; index < arrayQueryStringValues.Length; index++) {
					int current = sb.Length;
					sb.AddQueryParam(arrayQueryStringKey, arrayQueryStringValues[index]!);
					if (sb.Length > maxUrlLength - baseUrl.AbsoluteUri.Length) {
						sb.Length = current;
						if (index == 0) {
							throw new InvalidOperationException("Cannot create requests because url max length is smaller than the minimum length required for a single request");
						}
						break;
					}
				}
				urls.Add(sb.ToString());
				offset = index;
			} while (offset < arrayQueryStringValues.Length);
			return urls;
		}
		public static IEnumerable<string> CreateRequestUrlsByDelimitedQueryString(Uri baseUrl, string relativeUrl, NameValueCollection queryStrings, int maxUrlLength, string arrayQueryStringKey, string arrayQueryStringDelimiter, params string[] arrayQueryStringValues) {
			var urls = new List<string>();
			int offset = 0;
			do {
				var sb = relativeUrl.CreateUrl(queryStrings);
				sb.Append(Uri.EscapeDataString(arrayQueryStringKey)).Append('=');
				int index;
				for (index = offset; index < arrayQueryStringValues.Length; index++) {
					int current = sb.Length;
					if (index > offset) {
						sb.Append(Uri.EscapeDataString(arrayQueryStringDelimiter));
					}
					sb.Append(Uri.EscapeDataString(arrayQueryStringValues[index]));
					if (sb.Length > maxUrlLength - baseUrl.AbsoluteUri.Length) {
						sb.Length = current;
						if (index == 0) {
							throw new InvalidOperationException("Cannot create requests because url max length is smaller than the minimum length required for a single request");
						}
						break;
					}
				}
				urls.Add(sb.ToString());
				offset = index;
			} while (offset < arrayQueryStringValues.Length);
			return urls;
		}
		public static HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, RequestOptions options) {
			var sb = relativeUrl.CreateUrl(queryStringValues);
			if (sb.Length > 0 && sb[^1] == '&') { sb.Length--; }
			var request = new HttpRequestMessage(method, sb.ToString());
			// NoCache has been moved to DefaultRequestHeaders during client setup
			// request.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
			options.DataWriter.LogRequest(request);
			return request;
		}
		public static HttpRequestMessage CreateJsonRequest<T>(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, T? t, RequestOptions options) {
			var request = CreateRequest(method, relativeUrl, queryStringValues, options);
			if (t != null) {
				string content = JsonSerializer.Serialize(t, options.SerializerOptions);
				request.Content = new StringContent(content, Encoding.UTF8, ContentTypes.Json);
				options.DataWriter?.WriteLine(content);
			}
			return request;
		}
		public static HttpRequestMessage CreateStringRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, string? content, RequestOptions options) {
			var request = CreateRequest(method, relativeUrl, queryStringValues, options);
			if (content != null) {
				request.Content = new StringContent(content, Encoding.UTF8, ContentTypes.Text);
				options.DataWriter?.WriteLine(content);
			}
			return request;
		}
		public static HttpRequestMessage CreateStreamRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, Stream stream, RequestOptions options) {
			var request = CreateRequest(method, relativeUrl, queryStringValues, options);
			request.Content = new StreamContent(stream);
			return request;
		}
		public static HttpRequestMessage CreateMultiPartFormRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, RequestOptions options, params MultiPartFormData[] formDataArray) {
			var request = CreateRequest(method, relativeUrl, queryStringValues, options);
			var content = new MultipartFormDataContent();
			foreach (var item in formDataArray) {
				content.AddMultiPartFormData(item);
			}
			request.Content = content;
			return request;
		}
		public static HttpRequestMessage CreateFormUrlEncodedRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, IDictionary<string, string> formUrlEncodedValues, RequestOptions options) {
			var request = CreateRequest(method, relativeUrl, queryStringValues, options);
			var content = new FormUrlEncodedContent(formUrlEncodedValues);
			request.Content = content;
			return request;
		}
		#endregion

		#region get response
		public static async Task<string> GetRawResponse(this HttpRequestMessage request, HttpClient client, RequestOptions options) {
			options.Logger.LogDebug("{method}: {baseUri}{relativeUri}", request.Method, client.RequiredBaseUrl(), request.RequestUri);
			using (var response = await client.SendAsync(request, options.CancellationToken)) {
				return await response.ProcessResponseAsText(options);
			}
		}
		public static async Task<string> GetRawResponse<ErrorType>(this HttpRequestMessage request, HttpClient client, RequestOptions options) {
			options.Logger.LogDebug("{method}: {baseUri}{relativeUri}", request.Method, client.RequiredBaseUrl(), request.RequestUri);
			using (var response = await client.SendAsync(request, options.CancellationToken)) {
				return await response.ProcessResponseAsText<ErrorType>(options);
			}
		}
		public static async Task<ResultType?> GetJsonResponse<ResultType, ErrorType>(this HttpRequestMessage request, HttpClient client, RequestOptions options) {
			options.Logger.LogDebug("{method}: {baseUri}{relativeUri}", request.Method, client.RequiredBaseUrl(), request.RequestUri);
			using (var response = await client.SendAsync(request, options.CancellationToken)) {
				return await response.ProcessResponseAsJson<ResultType, ErrorType>(options);
			}
		}
		public static async Task<ResultType?> GetJsonResponse<ResultType>(this HttpRequestMessage request, HttpClient client, RequestOptions options) {
			options.Logger.LogDebug("{method}: {baseUri}{relativeUri}", request.Method, client.RequiredBaseUrl(), request.RequestUri);
			using (var response = await client.SendAsync(request, options.CancellationToken)) {
				return await response.ProcessResponseAsJson<ResultType>(options);
			}
		}
		public static async Task Download<ErrorType>(this HttpRequestMessage request, HttpClient client, Stream stream, RequestOptions options) {
			options.Logger.LogDebug("{method}: {baseUri}{relativeUri}", request.Method, client.RequiredBaseUrl(), request.RequestUri);
			using (var response = await client.SendAsync(request, options.CancellationToken)) {
				await response.Download<ErrorType>(stream, options);
			}
		}
		public static async Task Download(this HttpRequestMessage request, HttpClient client, Stream stream, RequestOptions options) {
			options.Logger.LogDebug("{method}: {baseUri}{relativeUri}", request.Method, client.RequiredBaseUrl(), request.RequestUri);
			using (var response = await client.SendAsync(request, options.CancellationToken)) {
				await response.Download(stream, options);
			}
		}
		public static async Task<ResultType> GetRequiredJsonResponse<ResultType>(this HttpRequestMessage request, HttpClient client, RequestOptions options) where ResultType : class
			=> await request.GetJsonResponse<ResultType>(client, options) ?? throw new InvalidDataException($"No data was returned from {request.Method}: {request.RequestUri}");

		public static async Task<ResultType> GetRequiredJsonResponseForValueType<ResultType>(this HttpRequestMessage request, HttpClient client, RequestOptions options) where ResultType : struct {
			var result = await request.GetJsonResponse<ResultType?>(client, options);
			return result ?? throw new InvalidDataException($"No data was returned from {request.Method}: {request.RequestUri}");
		}
		public static async Task<ResultType> GetRequiredJsonResponse<ResultType, ErrorType>(this HttpRequestMessage request, HttpClient client, RequestOptions options) {
			var result = await request.GetJsonResponse<ResultType, ErrorType>(client, options);
			return result ?? throw new InvalidDataException($"No data was returned from {request.Method}: {request.RequestUri}");
		}
		#endregion
	}
}