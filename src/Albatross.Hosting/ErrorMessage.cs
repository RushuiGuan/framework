using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public class ErrorMessage {
		public string Message { get; set; }
		public string Type { get; set; }
		public int StatusCode { get; set; }
	}
}
