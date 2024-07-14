using System;
using System.Collections;
using System.Collections.Generic;

namespace Albatross.CodeGen.Tests.Dto {
	public class MyDto {
		public string Name { get; set; } = string.Empty;
		public byte[] Data { get; set; } = new byte[0];

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
		public DateTime Date { get; set; }
		public DateTime DateTime { get; set; }
		public DateOnly DateOnly { get; set; }
		public DateTimeOffset DateTimeOffset { get; set; }
		public TimeSpan TimeSpan { get; set; }
		public Guid Guid { get; set; }
		public MyEnum Enum { get; set; }
		public MyStringEnum	StringEnum { get; set; }
		public int[] IntArray { get; set; } = new int[0];
		public IEnumerable IntEnumerable { get; set; } = new int[0];
		public IEnumerable<int> IntEnumerableGeneric { get; set; } = new int[0];
	}
}
