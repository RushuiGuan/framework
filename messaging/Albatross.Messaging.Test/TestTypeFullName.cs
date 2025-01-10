using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Messaging.Test {
	public class TestTypeFullName {
		public class GenericClass<T> {
		}
		[Fact]
		public void NormalCase() {
			Assert.NotNull(typeof(string).FullName);
		}

		[Fact]
		public void GenericTypeParameterCase() {
			var type = typeof(GenericClass<>)
			  .GetGenericArguments()
			  .First();
			Assert.Null(type.FullName);
			Assert.Null(type.Name);
		}


		[Fact]
		public void AnonymousTypeCase() {
			var anonymousType = new { Name = "Test", Value = 123 };
			Assert.Null(anonymousType.GetType().FullName);
		}
	}
}
