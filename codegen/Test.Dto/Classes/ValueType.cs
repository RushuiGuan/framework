using System.Text.Json;
using Test.Dto.Enums;

namespace Test.Dto.Classes {
	public record class ValueType {
		public int Int { get; set; }
		public long Long { get; set; }
		public double Double { get; set; }
		public decimal Decimal { get; set; }
		public float Float { get; set; }
		public bool Bool { get; set; }
		public char Char { get; set; }
		public short Short { get; set; }
		public ushort UShort { get; set; }
		public uint UInt { get; set; }
		public ulong ULong { get; set; }
		public sbyte SByte { get; set; }
		public byte Byte { get; set; }
		public DateTime DateTime { get; set; }
		public DateOnly DateOnly { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
		public TimeSpan TimeSpan { get; set; }
		public TimeOnly TimeOnly { get; set; }
		public Guid Guid { get; set; }
		public MyEnum Enum { get; set; }
		public MyStringEnum StringEnum { get; set; }
		public JsonElement JsonElement { get; set; }

		public int? NullableInt { get; set; }
		public long? NullableLong { get; set; }
		public double? NullableDouble { get; set; }
		public decimal? NullableDecimal { get; set; }
		public float? NullableFloat { get; set; }
		public bool? NullableBool { get; set; }
		public char? NullableChar { get; set; }
		public short? NullableShort { get; set; }
		public ushort? NullableUShort { get; set; }
		public uint? NullableUInt { get; set; }
		public ulong? NullableULong { get; set; }
		public sbyte? NullableSByte { get; set; }
		public byte? NullableByte { get; set; }
		public DateTime? NullableDateTime { get; set; }
		public DateOnly? NullableDateOnly { get; set; }
		public DateTimeOffset? NullableDateTimeOffset { get; set; }
		public TimeSpan? NullableTimeSpan { get; set; }
		public TimeOnly? NullableTimeOnly { get; set; }
		public Guid? NullableGuid { get; set; }
		public MyEnum? NullableEnum { get; set; }
		public MyStringEnum? NullableStringEnum { get; set; }
		public JsonElement? NullableJsonElement { get; set; }
	}
}