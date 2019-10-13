using NUnit.Framework;
using Albatross.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Albatross.Reflection.UnitTest {
    public struct GenericStruct<T> {
        public T ID { get; set; }
    }

    public class Tests {
        [Test]
        public void TestGetNullableValueType_GenericStruct() {
            Type type = typeof(GenericStruct<string>?);
            Assert.True(type.GetNullableValueType(out Type args));
            Assert.AreSame(typeof(GenericStruct<string>), args);
        }

        [Test]
        public void TestGetNullableValueType_True() {
            Type type = typeof(int?);
            Assert.True(type.GetNullableValueType(out Type args));
            Assert.AreSame(typeof(int), args);
        }

        [Test]
        public void TestGetNullableValueType_False() {
            Type type = typeof(int);
            Assert.False(type.GetNullableValueType(out Type args));
            Assert.Null(args);
        }

        [Test]
        public void TestGetCollectionElementType_True() {
            Type type = typeof(IEnumerable<string>);
            Assert.True(type.GetCollectionElementType(out Type args));
            Assert.AreSame(typeof(string), args);
        }

        [Test]
        public void TestGetCollectionElementType_False() {
            Type type = typeof(string);
            Assert.False(type.GetCollectionElementType(out Type args));
            Assert.Null(args);
        }

        [Test]
        public void TestIsAnonymousType_True() {
            Type type = new { a = 1 }.GetType();
            Assert.True(type.IsAnonymousType());
        }

        [Test]
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

        [Test]
        public void TestTryGetClosedGenericType_interface() {
            Type type = typeof(IEnumerable<>);
            Assert.True(typeof(Test1).TryGetClosedGenericType(typeof(IEnumerable<>), out Type generic));
            Assert.AreSame(typeof(IEnumerable<int>), generic);
        }

        [Test]
        public void TestTryGetClosedGenericType_class() {
            Assert.True(typeof(Test2).TryGetClosedGenericType(typeof(Base<>), out Type generic));
            Assert.AreSame(typeof(Base<string>), generic);
        }

        [Test]
        public void TestTryGetClosedGenericType_class_false_wrong_generic() {
            Assert.False(typeof(Test2).TryGetClosedGenericType(typeof(int), out Type generic));
            Assert.IsNull(generic);
        }

        [Test]
        public void TestTryGetClosedGenericType_class_false_wrong_type1() {
            Assert.False(typeof(Test2).TryGetClosedGenericType(typeof(IEnumerable<>), out Type generic));
            Assert.IsNull(generic);
        }

        [Test]
        public void TestTryGetClosedGenericType_class_false_wrong_type2() {
            Assert.False(typeof(Test2).TryGetClosedGenericType(typeof(Base1<>), out Type generic));
            Assert.IsNull(generic);
        }
    }
}