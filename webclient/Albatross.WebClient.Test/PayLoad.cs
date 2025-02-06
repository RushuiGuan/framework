using System;

namespace Albatross.WebClient.Test {
	public class PayLoad {
		public string Name { get; set; } = String.Empty;
		public byte[] Data { get; set; } = new byte[0];
		public int Number { get; set; }
		public DateTime Date { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
	}

	public static class PayLoadExtension {
		public static PayLoad Make() {
			return new PayLoad {
				Name = typeof(PayLoad).AssemblyQualifiedName!,
				Data = new byte[] { 0x22, 0x94, 0x12, 0x3 },
				Number = 939,
				Date = new DateTime(2019, 6, 29),
				DateTimeOffset = new DateTimeOffset(new DateTime(2019, 6, 2), TimeSpan.FromHours(-5)),
			};
		}
		public static string GetText() {
			return "Nervous realtor throws in second house.";
		}
	}
}