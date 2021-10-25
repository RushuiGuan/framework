using Albatross.Repository.Core;
using System.Linq;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestGeneral
	{
		class Instrument { 
			public string Name { get; set; }
		}

		[Fact]
		public void TestExceptionHandling() {
			Instrument[] array = new Instrument[0];
			try {
				array.AsQueryable().Item(nameof(Instrument.Name), "test");
			} catch (EntityNotFoundException err) {
				Assert.Equal("Cannot find Instrument with predicate: args.Name == \"test\"", err.Message);
			}

			try {
				string a = "test";
				array.AsQueryable().Item(nameof(Instrument.Name), a);
			} catch (EntityNotFoundException err) {
				Assert.Equal("Cannot find Instrument with predicate: args.Name == \"test\"", err.Message);
			}

			try {
				string a = null;
				array.AsQueryable().Item(nameof(Instrument.Name), a);
			} catch (EntityNotFoundException err) {
				Assert.Equal("Cannot find Instrument with predicate: args.Name == null", err.Message);
			}
		}
	}
}
