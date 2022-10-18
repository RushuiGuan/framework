using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.WebClient {
	public static class Extension {
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
			if (string.IsNullOrEmpty(data.Filename)) {
				content.Add(part, data.Name);
			} else {
				content.Add(part, data.Name, data.Filename);
			}
		}
	}
}