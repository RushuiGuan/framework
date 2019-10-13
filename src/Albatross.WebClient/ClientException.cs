using System;

namespace Albatross.WebClient{
	public class ClientException : Exception {
		public ErrorMessage ErrorMessage { get; private set; }

		public ClientException(string msg) : base(msg) {
			ErrorMessage = new ErrorMessage();
		}
		public ClientException(ErrorMessage error) : base(error.Message) {
			ErrorMessage = error;
		}
		public ClientException(string msg, Exception innerException) : base(msg, innerException) {
			ErrorMessage = new ErrorMessage {
				Message = msg,
			};
		}
	}
}
