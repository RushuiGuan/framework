using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.WebClient {
	public static class Extension {
		public static readonly HttpMethod HttpPatchMethod = new HttpMethod("PATCH");

		public static StringBuilder CreateUrl(this string url, NameValueCollection queryStringValues) {
			StringBuilder sb = new StringBuilder(url);
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
		static internal void AddQueryParam(this StringBuilder sb, string key, string value) {
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
			content.Add(part, data.Name, data.Filename);
		}


		#region get methods
		[Obsolete]
		public static async Task<T?> GetAsync<T>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateRequest(HttpMethod.Get, relativeUrl, queryStringValues)) {
				return await client.Invoke<T>(request, throwCustomException);
			}
		}

		[Obsolete]
		public static async Task<string> GetAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateRequest(HttpMethod.Get, relativeUrl, queryStringValues)) {
				return await client.Invoke(request, throwCustomException);
			}
		}
		#endregion

		#region delete
		[Obsolete]
		public static async Task DeleteAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateRequest(HttpMethod.Delete, relativeUrl, queryStringValues)) {
				await client.Invoke(request, throwCustomException);
			}
		}
		#endregion

		#region post
		[Obsolete]
		public static async Task<TOut?> PostAsync<TIn, TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateJsonRequest<TIn>(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task<TOut?> PostAsync<TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateStringRequest(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task<string> PostAsync<TIn>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateJsonRequest<TIn>(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task<string> PostAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateStringRequest(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request, throwCustomException);
			}
		}
		#endregion

		#region patch
		[Obsolete]
		public static async Task<TOut?> PatchAsync<TIn, TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateJsonRequest<TIn>(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task<TOut?> PatchAsync<TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateStringRequest(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task<string> PatchAsync<TIn>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateJsonRequest<TIn>(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task<string> PatchAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateStringRequest(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request, throwCustomException);
			}
		}
		#endregion

		#region put
		[Obsolete]
		public static async Task PutAsync<TIn>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input,  Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateJsonRequest<TIn>(HttpMethod.Put, relativeUrl, queryStringValues, input)) {
				await client.Invoke(request, throwCustomException);
			}
		}
		[Obsolete]
		public static async Task PutAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			using (var request = client.CreateStringRequest(HttpMethod.Put, relativeUrl, queryStringValues, input)) {
				await client.Invoke(request, throwCustomException);
			}
		}
		#endregion
	}
}