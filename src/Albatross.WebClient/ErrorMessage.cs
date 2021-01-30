using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.WebClient {
	public class ErrorMessage {
		public string Message { get; set; }
		public string Type { get; set; }
		public int HttpStatus { get; set; }
	}
}