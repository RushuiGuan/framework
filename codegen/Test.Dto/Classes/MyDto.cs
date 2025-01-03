using System.Collections;
using System.Text.Json.Serialization;
using Test.Dto.Enums;

namespace Test.Dto.Classes {
	public class MyDto {
		public string Name { get; set; } = string.Empty;
		public string StringLongName { get; set; } = string.Empty;
		public byte[] ByteArray { get; set; } = new byte[0];

		public int Int { get; set; }
		public long Long { get; set; }
		public double Double { get; set; }
		public decimal Decimal { get; set; }
		public decimal DecimalLongName { get; set; }
		public float Float { get; set; }
		public bool Bool { get; set; }
		public char Char { get; set; }
		public char[] CharArray { get; set; } = new char[0];
		public short Short { get; set; }
		public ushort UShort { get; set; }
		public uint UInt { get; set; }
		public ulong ULong { get; set; }
		public sbyte SByte { get; set; }
		public byte Byte { get; set; }
		public DateTime Date { get; set; }
		public DateTime DateTime { get; set; }
		public DateOnly DateOnly { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
		public TimeSpan TimeSpan { get; set; }
		public int? NullableInt { get; set; }
		public int? NullableIntGeneric { get; set; }
		public string? NullableString { get; set; }
		public string?[] NullableStringArray { get; set; } = new string[0];
		public Guid Guid { get; set; }
		public MyEnum Enum { get; set; }
		public MyStringEnum StringEnum { get; set; }
		public int[] IntArray { get; set; } = new int[0];
		public IEnumerable IntEnumerable { get; set; } = new int[0];
		public IEnumerable<int> IntEnumerableGeneric { get; set; } = new int[0];

		[JsonIgnore]
		public string Ignored { get; set; } = string.Empty;
	}
}