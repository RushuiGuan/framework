﻿namespace Test.Dto {
	public class MyClassWithGenericBaseType : GenericBaseType<string> {
		public MyClassWithGenericBaseType(string name, string value) : base(name, value) {
		}
	}
}