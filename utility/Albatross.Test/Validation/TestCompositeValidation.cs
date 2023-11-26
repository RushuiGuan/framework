using Albatross.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Albatross.Test.Validation {

	public class Parent {
		[RecursiveValidation, Required]
		public Child Child { get; set; }
	}

	public class Child {
		[Required]
		public string Name { get; set; }
	}

	public class TestValidation {
		[Fact]
		public void TestChildObjectValidation() {
			var parent = new Parent {
				Child = new Child(),
			};
			//validation should kick in for the child object
			Assert.Throws<ValidationException>(() => Validator.ValidateObject(parent, new ValidationContext(parent), true));
		}
	}
}
