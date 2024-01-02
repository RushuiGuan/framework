using System;

namespace Albatross.CodeGen.Tests.Dto {
	public class MyDto {
		public string Name { get; set; } = string.Empty;
		public byte[] Data { get; set; } = new byte[0];
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
	}
}
