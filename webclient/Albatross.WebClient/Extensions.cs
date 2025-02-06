using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Albatross.WebClient.Test")]
namespace Albatross.WebClient {
	public static class Extensions {
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
			if (string.IsNullOrEmpty(data.Filename)) {
				content.Add(part, data.Name);
			} else {
				content.Add(part, data.Name, data.Filename);
			}
		}

		public static async Task<string> ReadResponseAsText(this HttpResponseMessage response) {
			if (response.Content.Headers.ContentLength != 0 == true) {
				if (response.Content.Headers.ContentEncoding.Contains(ClientBase.GZipEncoding)) {
					var stream = await response.Content.ReadAsStreamAsync();
					if (stream.CanSeek) {
						stream.Seek(0, SeekOrigin.Begin);
					}
					var gzip = new GZipStream(stream, CompressionMode.Decompress);
					var reader = new StreamReader(gzip);
					return await reader.ReadToEndAsync();
				} else {
					return await response.Content.ReadAsStringAsync();
				}
			} else {
				return string.Empty;
			}
		}

		public static async Task LogResponse(this TextWriter? writer, HttpResponseMessage response, bool logContent = true) {
			if (writer != null) {
				writer.WriteLine("-------------------- Response --------------------");
				writer.WriteLine($"status-code: {response.StatusCode}({(int)response.StatusCode})");
				LogHeader(writer, response.Headers);
				LogHeader(writer, response.Content.Headers);
				if (logContent) {
					writer.WriteLine("-------------------- Content --------------------");
					var content = await response.ReadResponseAsText();
					if (!content.EndsWith('\n')) {
						writer.WriteLine(content);
					} else {
						writer.Write(content);
					}
				}
			}
		}
		static void LogHeader(TextWriter myWriter, HttpHeaders headers) {
			foreach (var header in headers) {
				myWriter.Write(header.Key);
				myWriter.Write(":");
				foreach (var value in header.Value) {
					myWriter.Write(" ");
					myWriter.Write(value);
				}
				myWriter.WriteLine();
			}
		}
		public static void LogRequest(this TextWriter? writer, HttpRequestMessage request, HttpClient client) {
			if (writer != null) {
				writer.WriteLine("-------------------- Request --------------------");
				writer.Write(request.Method);
				writer.Write(" ");
				writer.WriteLine(request.RequestUri);
				writer.WriteLine("-------------------- Headers --------------------");
				LogHeader(writer, client.DefaultRequestHeaders);
				LogHeader(writer, request.Headers);
				if (request.Content != null) {
					LogHeader(writer, request.Content.Headers);
					writer.WriteLine("-------------------- Content --------------------");
					string text = request.Content.ReadAsStringAsync().Result;
					writer.WriteLine(text);
				}
				writer.WriteLine();
			}
		}

		public static async Task<ResultType> GetRequiredJsonResponse<ResultType>(this ClientBase client, HttpRequestMessage request) where ResultType : class {
			var result = await client.GetJsonResponse<ResultType>(request);
			return result ?? throw new InvalidDataException($"No data was returned from {request.Method}: {request.RequestUri}");
		}
		public static async Task<ResultType> GetRequiredJsonResponseForValueType<ResultType>(this ClientBase client, HttpRequestMessage request) where ResultType : struct {
			var result = await client.GetJsonResponse<ResultType?>(request);
			return result ?? throw new InvalidDataException($"No data was returned from {request.Method}: {request.RequestUri}");
		}

		public static async Task<ResultType> GetRequiredJsonResponse<ResultType, ErrorType>(this ClientBase client, HttpRequestMessage request) {
			var result = await client.GetJsonResponse<ResultType, ErrorType>(request);
			return result ?? throw new InvalidDataException($"No data was returned from {request.Method}: {request.RequestUri}");
		}
	}
}