using Albatross.CodeGen.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Variable : ICodeElement{
		public Variable(string name) {
			this.Name = name;
		}
		public string Name { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Write(this.Name);
			return writer;
		}
	}
}
