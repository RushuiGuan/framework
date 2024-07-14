using System;

namespace Albatross.CodeGen.Tests.Dto {
	public class MyBase {
		public int Id { get; set; }
	}
	public class MyBase<T> {
		public string Name { get; set; }
		public MyBase(string name) {
			Name = name;
		}
	}
}
