using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace Albatross.WebClient {
	/// <summary>
	/// if the webclient is being used to talk to web api hosted by Albatross.Hosting, by default
	/// Albatross.Hosting will throw exception using the json format below, camel cased.
	/// </summary>
	public class ServiceError {
		public string Message { get; set; }
		[JsonPropertyName("type")]  // The server side serialize this property as "type"
		public string? ClassName { get; set; }

		public ServiceError(string message) {
			this.Message = message;
		}
	}


	public class ServiceException : ServiceException<ServiceError> {
		public ServiceException(HttpStatusCode status, HttpMethod? method, Uri? endpoint, ServiceError? detail, string errorMsg)
			: base(status, method, endpoint, detail, errorMsg) { }
	}

	public class ServiceException<T> : Exception {
		public HttpStatusCode StatusCode { get; private set; }
		public string? Method { get; set; }
		public string? Endpoint { get; set; }
		public T? ErrorObject { get; private set; }

		public ServiceException(HttpStatusCode statusCode, HttpMethod? method, Uri? endpoint, T? errorObject, string errorMsg)
			: base(BuildMessage(statusCode, method, endpoint, errorMsg)) {

			this.StatusCode = statusCode;
			this.Method = method?.ToString();
			this.Endpoint = endpoint?.ToString();
			this.ErrorObject = errorObject;
		}

		/*
		 * sample log below:
		 * 500 GET: http://mywebsite/test/api/entity/schedule/1
		 * Message: the actual error message
		 */
		static string BuildMessage(HttpStatusCode statusCode, HttpMethod? method, Uri? endpoint, string errorMsg) {
			var sb = new StringBuilder();
			sb.Append((int)statusCode).Append(" ").Append(method).Append(": ").Append(endpoint?.ToString());
			if (!string.IsNullOrEmpty(errorMsg)) {
				sb.AppendLine().Append(errorMsg);
			}
			return sb.ToString();
		}
	}
}