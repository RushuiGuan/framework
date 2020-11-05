using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Albatross.WebClient {
	public static class Extension {
		public static readonly HttpMethod HttpPatchMethod = new HttpMethod("PATCH");

		public static string GetUrl(this string url, NameValueCollection queryStringValues) {
			StringWriter writer = new StringWriter();
			writer.Write(url);
			if (queryStringValues?.Count > 0) {
				writer.Write("?");
				for (int i = 0; i < queryStringValues.Count; i++) {
					string[] values = queryStringValues.GetValues(i);
					string key = queryStringValues.GetKey(i);
					foreach (string value in values ??new string[0]) {
						writer.Write(Uri.EscapeDataString(key));
						writer.Write("=");
						writer.Write(Uri.EscapeDataString(value));
						writer.Write("&");
					}
				}
			}
			return writer.ToString();
		}

		public static string GetUrl(this string url, IEnumerable<KeyValuePair<string, string>> queryStringValues) {
			StringWriter writer = new StringWriter();
			writer.Write(url);
			if (queryStringValues?.Count() > 0) {
				writer.Write("?");
				foreach (var item in queryStringValues) {
					writer.Write(Uri.EscapeDataString(item.Key));
					writer.Write("=");
					writer.Write(Uri.EscapeDataString(item.Value));
					writer.Write("&");
				}
			}
			return writer.ToString();
		}

		#region get methods
		public static async Task<T> GetAsync<T>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues) {
			using (var request = client.CreateRequest(HttpMethod.Get, relativeUrl, queryStringValues)) {
				return await client.Invoke<T>(request);
			}
		}

		public static async Task<string> GetAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues) {
			using (var request = client.CreateRequest(HttpMethod.Get, relativeUrl, queryStringValues)) {
				return await client.Invoke(request);
			}
		}
		#endregion

		#region delete
		public static async Task DeleteAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues) {
			using (var request = client.CreateRequest(HttpMethod.Delete, relativeUrl, queryStringValues)) {
				await client.Invoke(request);
			}
		}
		#endregion

		#region post
		public static async Task<TOut> PostAsync<TIn, TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input) {
			using (var request = client.CreateJsonRequest<TIn>(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request);
			}
		}
		public static async Task<TOut> PostAsync<TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input) {
			using (var request = client.CreateStringRequest(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request);
			}
		}
		public static async Task<string> PostAsync<TIn>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input) {
			using (var request = client.CreateJsonRequest<TIn>(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request);
			}
		}
		public static async Task<string> PostAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input) {
			using (var request = client.CreateStringRequest(HttpMethod.Post, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request);
			}
		}
		#endregion

		#region patch
		public static async Task<TOut> PatchAsync<TIn, TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input) {
			using (var request = client.CreateJsonRequest<TIn>(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request);
			}
		}
		public static async Task<TOut> PatchAsync<TOut>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input) {
			using (var request = client.CreateStringRequest(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke<TOut>(request);
			}
		}
		public static async Task<string> PatchAsync<TIn>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input) {
			using (var request = client.CreateJsonRequest<TIn>(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request);
			}
		}
		public static async Task<string> PatchAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input) {
			using (var request = client.CreateStringRequest(HttpPatchMethod, relativeUrl, queryStringValues, input)) {
				return await client.Invoke(request);
			}
		}
		#endregion

		#region put
		public static async Task PutAsync<TIn>(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, TIn input) {
			using (var request = client.CreateJsonRequest<TIn>(HttpMethod.Put, relativeUrl, queryStringValues, input)) {
				await client.Invoke(request);
			}
		}
		public static async Task PutAsync(this ClientBase client, string relativeUrl, NameValueCollection queryStringValues, string input) {
			using (var request = client.CreateStringRequest(HttpMethod.Put, relativeUrl, queryStringValues, input)) {
				await client.Invoke(request);
			}
		}
		#endregion
	}
}