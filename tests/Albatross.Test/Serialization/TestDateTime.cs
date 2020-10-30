using Albatross.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Albatross.Test.Serialization {
	public class TestDateTime {
		[Fact]
		public void RunSample() {
			string text = string.Format("{0:o}", DateTime.Now);
			Assert.NotNull(text);

			text = string.Format("{0:o}", new DateTime(2020, 1, 1, 8, 30, 40));
			Assert.NotNull(text);

			text = string.Format("{0:o}", DateTime.SpecifyKind(new DateTime(2020, 1, 1, 8, 30, 40), DateTimeKind.Utc));
			Assert.NotNull(text);
		}
	}
}
