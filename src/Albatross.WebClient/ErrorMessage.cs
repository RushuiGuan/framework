using System.Net;

namespace Albatross.WebClient {
	public class ErrorMessage {
		public ErrorMessage() { }

		public ErrorMessage(HttpStatusCode statusCode, string msg) {
			this.StatusCode = statusCode;
			this.Message = msg;
		}

		public string Message { get; set; }
		public string Type { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
}