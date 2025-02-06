using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.RestClient {
	public static class ResponseExtensions {
		public const string GZipEncoding = "gzip";


		public static async Task<string> ReadResponseAsText(this HttpResponseMessage response) {
			if (response.Content.Headers.ContentLength != 0 == true) {
				if (response.Content.Headers.ContentEncoding.Contains(GZipEncoding)) {
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
		public static Task EnsureStatusCode(this HttpResponseMessage response, JsonSerializerOptions serializerOptions) => EnsureStatusCode<ServiceError>(response, serializerOptions);
		public static async Task EnsureStatusCode<ErrorType>(this HttpResponseMessage response, JsonSerializerOptions serializerOptions) {
			Exception exception;
			if ((int)response.StatusCode > 399) {
				string content = await response.ReadResponseAsText();
				try {
					var error = JsonSerializer.Deserialize<ErrorType>(content, serializerOptions);
					if (typeof(ErrorType) == typeof(ServiceError)) {
						exception = new ServiceException(response.StatusCode, response.RequestMessage?.Method, response.RequestMessage?.RequestUri, error as ServiceError, content);
					} else {
						exception = new ServiceException<ErrorType>(response.StatusCode, response.RequestMessage?.Method, response.RequestMessage?.RequestUri, error, content);
					}
				} catch {
					exception = new ServiceException(response.StatusCode, response.RequestMessage?.Method, response.RequestMessage?.RequestUri, null, content);
				}
				throw exception;
			}
		}
		public static async Task<T?> ReadResponseAsJson<T>(this HttpResponseMessage response, JsonSerializerOptions serializerOptions) {
			if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0) {
				return default;
			} else {
				var stream = await response.Content.ReadAsStreamAsync();
				if (response.Content.Headers.ContentEncoding.Contains(GZipEncoding)) {
					var gzip = new GZipStream(stream, CompressionMode.Decompress);
					return await JsonSerializer.DeserializeAsync<T>(gzip, serializerOptions);
				} else {
					return await JsonSerializer.DeserializeAsync<T>(stream, serializerOptions);
				}
			}
		}
		public static async Task<string> ProcessResponseAsText(this HttpResponseMessage response, RequestOptions options) {
			try {
				await EnsureStatusCode(response, options.SerializerOptions);
				string content = await response.ReadResponseAsText();
				return content;
			} finally {
				await options.DataWriter.LogResponse(response);
			}
		}
		public static async Task<string> ProcessResponseAsText<ErrorType>(this HttpResponseMessage response, RequestOptions options) {
			try {
				await EnsureStatusCode<ErrorType>(response, options.SerializerOptions);
				string content = await response.ReadResponseAsText();
				return content;
			} finally {
				await options.DataWriter.LogResponse(response);
			}
		}
		public static async Task<ResultType?> ProcessResponseAsJson<ResultType, ErrorType>(this HttpResponseMessage response, RequestOptions options) {
			try {
				await EnsureStatusCode<ErrorType>(response, options.SerializerOptions);
				var result = await ReadResponseAsJson<ResultType>(response, options.SerializerOptions);
				return result;
			} finally {
				await options.DataWriter.LogResponse(response);
			}
		}
		public static async Task<ResultType?> ProcessResponseAsJson<ResultType>(this HttpResponseMessage response, RequestOptions options) {
			try {
				await EnsureStatusCode(response, options.SerializerOptions);
				string content = await response.ReadResponseAsText();
				return response.StatusCode == HttpStatusCode.NoContent ? default(ResultType) : JsonSerializer.Deserialize<ResultType>(content);
			} finally {
				await options.DataWriter.LogResponse(response);
			}
		}
		public static async Task Download<ErrorType>(this HttpResponseMessage response, Stream stream, RequestOptions options) {
			try {
				await EnsureStatusCode<ErrorType>(response, options.SerializerOptions);
				if (response.Content.Headers.ContentEncoding.Contains(GZipEncoding)) {
					using var responseStream = await response.Content.ReadAsStreamAsync();
					using var gzip = new GZipStream(responseStream, CompressionMode.Decompress);
					await gzip.CopyToAsync(stream);
				} else {
					await response.Content.CopyToAsync(stream);
				}
			} finally {
				await options.DataWriter.LogResponse(response, false);
			}
		}
		public static Task Download(this HttpResponseMessage response, Stream stream, RequestOptions options) => Download<ServiceError>(response, stream, options);
	}
}
