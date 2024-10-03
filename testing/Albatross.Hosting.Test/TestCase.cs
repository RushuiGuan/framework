namespace Albatross.Hosting.Test {
	public class TestCase {
		public string Name { get; init; }
		public TestCase(string name) {
			Name = name;
		}
	}

	public class TestCase<T> : TestCase{
		public T? P1 { get; set; }
		public TestCase(string name):base(name) {		}
	}
	public class TestCase<T1, T2> :TestCase{
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public TestCase(string name):base(name) {		}
	}
	public class TestCase<T1, T2, T3> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4, T5> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public T5? P5 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4, T5, T6> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public T5? P5 { get; set; }
		public T6? P6 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4, T5, T6, T7> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public T5? P5 { get; set; }
		public T6? P6 { get; set; }
		public T7? P7 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4, T5, T6, T7, T8> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public T5? P5 { get; set; }
		public T6? P6 { get; set; }
		public T7? P7 { get; set; }
		public T8? P8 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4, T5, T6, T7, T8, T9> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public T5? P5 { get; set; }
		public T6? P6 { get; set; }
		public T7? P7 { get; set; }
		public T8? P8 { get; set; }
		public T9? P9 { get; set; }
		public TestCase(string name) : base(name) { }
	}
	public class TestCase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : TestCase {
		public T1? P1 { get; set; }
		public T2? P2 { get; set; }
		public T3? P3 { get; set; }
		public T4? P4 { get; set; }
		public T5? P5 { get; set; }
		public T6? P6 { get; set; }
		public T7? P7 { get; set; }
		public T8? P8 { get; set; }
		public T9? P9 { get; set; }
		public T10? P10 { get; set; }
		public TestCase(string name) : base(name) { }
	}
}
