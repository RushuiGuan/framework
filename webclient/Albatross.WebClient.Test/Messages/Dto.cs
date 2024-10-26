using System;

namespace Albatross.WebClient.Test.Messages {
	public class Dto {
		public string Name { get; set; } = string.Empty;
		public byte[] Data { get; set; } = new byte[0];
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
	}
}