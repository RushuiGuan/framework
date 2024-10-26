using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.WebClient {
	public class MultiPartFormData {
		public MultiPartFormData(string name, string contentType) {
			this.Name = name;
			this.ContentType = contentType;
		}
		public string Name { get; set; }
		public byte[] Data { get; set; } = new byte[0];
		public Stream? Stream { get; set; }
		public string ContentType { get; set; }
		public string? Filename { get; set; }
	}
}