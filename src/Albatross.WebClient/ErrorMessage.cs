using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.WebClient {
	public class ErrorMessage {
		public string Message { get; set; }
		public string ExceptionMessage { get; set; }
		public string ExceptionType { get; set; }
		public string StackTrace { get; set; }
		public int? StatusCode { get; set; }
		public string StatusDescription { get; set; }
	}
}