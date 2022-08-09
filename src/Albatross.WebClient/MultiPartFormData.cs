using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.WebClient {
	public class MultiPartFormData {
		public MultiPartFormData(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public byte[] Data { get; set; } = new byte[0];
		public Stream? Stream { get; set; }
	}
}
