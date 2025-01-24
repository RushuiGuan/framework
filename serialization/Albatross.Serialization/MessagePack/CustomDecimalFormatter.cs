using Albatross.Text;
using MessagePack;
using MessagePack.Formatters;

namespace Albatross.Serialization.MessagePack {
	public sealed class CustomDecimalFormatter : IMessagePackFormatter<decimal> {
		public static readonly CustomDecimalFormatter Instance = new CustomDecimalFormatter();

		private CustomDecimalFormatter() { }

		public void Serialize(ref MessagePackWriter writer, decimal value, MessagePackSerializerOptions options) {
			writer.Write(value.Decimal2CompactText());
		}

		public decimal Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options) {
			var text = reader.ReadString();
			if (string.IsNullOrEmpty(text)) {
				return 0;
			} else {
				return decimal.Parse(text);
			}
		}
	}
}