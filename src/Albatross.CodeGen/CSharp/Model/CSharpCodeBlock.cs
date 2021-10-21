using Albatross.CodeGen.Core;
using System;
namespace Albatross.CodeGen.CSharp.Model {
	public class CSharpCodeBlock : ICodeBlock{
		public CSharpCodeBlock() { }
		public CSharpCodeBlock(string content) {
			this.Content = content;
		}
		public string Content { get; set; } = string.Empty;
	}
}
