using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Class {
		public Class(string name) {
			Name = name;
		}
		public Class() { }

		public AccessModifier AccessModifier { get; set; }
		public string Name { get; set; }
		public Class BaseClass { get; set; }
		public bool Static { get; set; }
        public bool Sealed { get; set; }
		public bool Abstract { get; set; }
		public bool Partial { get; set; }
		public string Namespace { get; set; }
		public bool IsGeneric { get; set; }

		public IEnumerable<string> Imports { get; set; }
		public IEnumerable<Constructor> Constructors { get; set; }
		public IEnumerable<Property> Properties { get; set; }
		public IEnumerable<Field> Fields { get; set; }
		public IEnumerable<Method> Methods { get; set; }
	}
}
