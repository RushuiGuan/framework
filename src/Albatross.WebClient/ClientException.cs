using System;
using System.Net;

namespace Albatross.WebClient{
	public class ClientException : Exception {
		public ErrorMessage ErrorMessage { get; }
		
		public ClientException(HttpStatusCode statusCode, string msg) : this(new ErrorMessage(statusCode, msg)) {
		}

		public ClientException(ErrorMessage err):base($"{err.StatusCode}({(int)err.StatusCode}):{err.Message}") {
			ErrorMessage = err;
		}
	}
}
