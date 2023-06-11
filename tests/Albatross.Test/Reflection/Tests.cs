using Albatross.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;
using Xunit;
using System.IO;
using System.Linq;
using Albatross.Config;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Agreement.JPake;

namespace Albatross.Test.Reflection {
    public struct GenericStruct<T> {
        public T ID { get; set; }
    }

    public class Tests {
        [Fact]
        public void TestGetNullableValueType_GenericStruct() {
            Type type = typeof(GenericStruct<string>?);
            Assert.True(type.GetNullableValueType(out Type args));
            Assert.Same(typeof(GenericStruct<string>), args);
        }

        [Fact]
        public void TestGetNullableValueType_True() {
            Type type = typeof(int?);
            Assert.True(type.GetNullableValueType(out Type args));
            Assert.Same(typeof(int), args);
        }

        [Fact]
        public void TestGetNullableValueType_False() {
            Type type = typeof(int);
            Assert.False(type.GetNullableValueType(out Type args));
            Assert.Null(args);
        }


		[Fact]
		public void TestGetTestResultType_True() {
			Type type = typeof(Task<string>);
			Assert.True(type.GetTaskResultType(out Type args));
			Assert.Same(typeof(string), args);
		}

		[Fact]
		public void TestGetTestResultType_False() {
			Type type = typeof(string);
			Assert.False(type.GetTaskResultType(out Type args));
			Assert.Null(args);
		}

		[Fact]
        public void TestGetCollectionElementType_True() {
            Type type = typeof(IEnumerable<string>);
            Assert.True(type.GetCollectionElementType(out Type args));
            Assert.Same(typeof(string), args);
        }

        [Fact]
        public void TestGetCollectionElementType_False() {
            Type type = typeof(string);
            Assert.False(type.GetCollectionElementType(out Type args));
            Assert.Null(args);
        }

        [Fact]
        public void TestIsAnonymousType_True() {
            Type type = new { a = 1 }.GetType();
            Assert.True(type.IsAnonymousType());
        }

        [Fact]
        public void TestIsAnonymousType_False() {
            Type type = typeof(int);
            Assert.False(type.IsAnonymousType());
        }

        public class Test1 : IEnumerable<int> {
            public IEnumerator<int> GetEnumerator() {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                throw new NotImplementedException();
            }
        }
        public class Base<T> { }
        public class Base1<T> { }
        public class Test2 : Base<string> { }

        [Fact]
        public void TestTryGetClosedGenericType_interface() {
            Type type = typeof(IEnumerable<>);
            Assert.True(typeof(Test1).TryGetClosedGenericType(typeof(IEnumerable<>), out Type generic));
            Assert.Same(typeof(IEnumerable<int>), generic);
        }

        [Fact]
        public void TestTryGetClosedGenericType_class() {
            Assert.True(typeof(Test2).TryGetClosedGenericType(typeof(Base<>), out Type generic));
            Assert.Same(typeof(Base<string>), generic);
        }

        [Fact]
        public void TestTryGetClosedGenericType_class_false_wrong_generic() {
            Assert.False(typeof(Test2).TryGetClosedGenericType(typeof(int), out Type generic));
            Assert.Null(generic);
        }

        [Fact]
        public void TestTryGetClosedGenericType_class_false_wrong_type1() {
            Assert.False(typeof(Test2).TryGetClosedGenericType(typeof(IEnumerable<>), out Type generic));
            Assert.Null(generic);
        }

        [Fact]
        public void TestTryGetClosedGenericType_class_false_wrong_type2() {
            Assert.False(typeof(Test2).TryGetClosedGenericType(typeof(Base1<>), out Type generic));
            Assert.Null(generic);
        }

		[Fact]
		public void TestGetResource() {
			string text = this.GetType().GetEmbeddedFile("test.txt");
			Assert.Equal("test", text);
		}

		[Fact]
		public void TestAssemblyLocation() {
			string location = this.GetType().Assembly.Location;
			string name1 = new FileInfo(location).Directory?.FullName;
			string name2 = new FileInfo(location).DirectoryName;
			Assert.Equal(name1, name2);
		}

		[Fact]
		public void TestAssemblyCodeBase() {
			string location = this.GetType().Assembly.Location;
			location = System.IO.Path.GetDirectoryName(location);
			string actual = this.GetType().GetAssemblyLocation();
			Assert.Equal(location, actual);
		}

		[Fact]
		public void TestIsNullable() {
			Assert.True(typeof(int?).IsNullable());
			Assert.False(this.GetType().IsNullable());
		}

		[Fact]
		public void TestIsDerived() {
			Assert.True(typeof(string).IsDerived<object>());
			Assert.False(typeof(object).IsDerived<string>());

			Assert.True(typeof(string).IsDerived(typeof(object)));
			Assert.False(typeof(object).IsDerived(typeof(string)));
		}

		class Style {
			public string? Name { get; set; }
			public string? Color { get; set; }
			public Width? Width { get; set; }
			public Padding? Padding { get; set; }
		}

		class Width {
			public string? Unit { get; set; }
			public int Number { get; set; }
			public int? Stroke { get; set; }
		}
		class Padding {
			public string? Left { get; set; }
			public string? Right { get; set; }
		}
	
		[Theory]
		[InlineData(nameof(Style.Name), "box")]
		[InlineData(nameof(Style.Color), "red")]
		[InlineData("Width.Unit", "px")]
		[InlineData("Width.Number", 100)]
		[InlineData("Width.Stroke", null)]
		[InlineData("Padding", null)]
		[InlineData("Padding.Left", null)]
		[InlineData("Padding.Right", null)]
		[InlineData("Padding.x", null)]	// the method will return null here even when x is not a valid property because the Padding property is null
		public void TestGetPropertyValue(string propertyName, object expected) {
			var obj = new Style {
				Name = "box",
				Color = "red",
				Width = new Width {
					Unit = "px",
					Number = 100,
				}
			};
			Assert.Equal(typeof(Style).GetPropertyValue(obj, propertyName), expected);
		}
		[Theory]
		[InlineData("Name1")]
		[InlineData("Color1")]
		[InlineData("Width.Unit1")]
		public void TestGetPropertyValueNotFound(string propertyName) {
			var obj = new Style {
				Name = "box",
				Color = "red",
				Width = new Width {
					Unit = "px",
					Number = 100,
				}
			};
			Assert.Throws<ArgumentException>(() => typeof(Style).GetPropertyValue(obj, propertyName));
		}

		[Theory]
		[InlineData("Albatross.Test.Reflection.Tests,xxxxxx", "Albatross.Test.Reflection.Tests,xxxxxx")]
		public void TestGetClassNameNeat(string className, string expected) {
			var result = className.GetClass().GetClassNameNeat();
			Assert.Equal(expected, result);
		}
	}
}