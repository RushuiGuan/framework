using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestFileMove {
		[Fact]
		public void MoveWithOverWrite() {
			var source = Path.GetTempFileName();
			var destination = Path.GetTempFileName();
			File.WriteAllText(source, "Hello World");
			File.WriteAllText(destination, "Goodbye World");
			IO.Extensions.MoveFileWithOverwrite(source, destination);
			var text = File.ReadAllText(destination);
			Assert.Equal("Hello World", text);
			Assert.False(File.Exists(source));
		}

		[Fact]
		public void MoveWithoutOverwrite(){
			var source = Path.GetTempFileName();
			var destination = Path.GetTempFileName();
			File.Delete(destination);
			File.WriteAllText(source, "Hello World");
			IO.Extensions.MoveFileWithOverwrite(source, destination);
			var text = File.ReadAllText(destination);
			Assert.Equal("Hello World", text);
			Assert.False(File.Exists(source));
		}
	}
}
