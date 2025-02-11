using Albatross.Reflection;
using Albatross.Text.Table;
using AutoFixture;

namespace Sample {
	internal class Program {
		static void Main(string[] args) {
			var builder = new TableOptionBuilder<Contact>().AddPropertiesByReflection();
			var options = new TableOptions<Contact>(builder);
			var fixture = new Fixture();
			var contacts = fixture.CreateMany<Contact>(20);
			contacts.StringTable()
				.MinWidth(x => true, 20)
				.PrintConsole();
		}
	}
}
