using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestASCIISortedFileOperation {

		[Fact]
		public void Run() {
			AsciiExtensions.StitchChanges<DateTime>("a.csv", "b.csv", x=> DateTime.Parse(x));
		}
	}
}
