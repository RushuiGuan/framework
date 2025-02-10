﻿using Albatross.Text.Table;
using AutoFixture;

namespace Sample {
	internal class Program {
		static void Main(string[] args) {
			
			var builder = new TableOptionBuilder<Contact>().SetByReflection();



			var options = new TableOptions<Contact>(builder);
			var fixture = new Fixture();
			var contacts = fixture.CreateMany<Contact>(20);
			contacts.Console(true, options);
		}
	}
}
