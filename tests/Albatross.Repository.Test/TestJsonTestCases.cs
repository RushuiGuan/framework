using Albatross.Hosting.Test;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestJsonTestCases {

		int[] ParseName(string name) {
			return name.Split(':')[1].Split(' ').Select(args => int.Parse(args)).ToArray();
		}


		[Theory]
		[InlineData("test param:1")]
		public void TestParam1(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
		}

		[Theory]
		[InlineData("test param:1 2")]
		public void TestParam2(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
		}

		[Theory]
		[InlineData("test param:1 2 3")]
		public void TestParam3(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
		}

		[Theory]
		[InlineData("test param:1 2 3 4")]
		public void TestParam4(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
		}

		[Theory]
		[InlineData("test param:1 2 3 4 5")]
		public void TestParam5(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
			Assert.Equal(array[4], value.P5);
		}

		[Theory]
		[InlineData("test param:1 2 3 4 5 6")]
		public void TestParam6(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
			Assert.Equal(array[4], value.P5);
			Assert.Equal(array[5], value.P6);
		}


		[Theory]
		[InlineData("test param:1 2 3 4 5 6 7")]
		public void TestParam7(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int, int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
			Assert.Equal(array[4], value.P5);
			Assert.Equal(array[5], value.P6);
			Assert.Equal(array[6], value.P7);
		}

		[Theory]
		[InlineData("test param:1 2 3 4 5 6 7 8")]
		public void TestParam8(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int, int, int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
			Assert.Equal(array[4], value.P5);
			Assert.Equal(array[5], value.P6);
			Assert.Equal(array[6], value.P7);
			Assert.Equal(array[7], value.P8);
		}

		[Theory]
		[InlineData("test param:1 2 3 4 5 6 7 8 9")]
		public void TestParam9(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int, int, int, int, int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
			Assert.Equal(array[4], value.P5);
			Assert.Equal(array[5], value.P6);
			Assert.Equal(array[6], value.P7);
			Assert.Equal(array[7], value.P8);
			Assert.Equal(array[8], value.P9);
		}

		[Theory]
		[InlineData("test param:1 2 3 4 5 6 7 8 9 10")]
		public void TestParam10(string testCaseName) {
			var value = Assembly.GetExecutingAssembly().GetJsonTestCases<int, int, int, int, int, int, int, int , int, int>("Albatross.Repository.Test.TestCases.data.json", testCaseName);
			Assert.NotNull(value);
			var array = ParseName(testCaseName);
			Assert.Equal(array[0], value.P1);
			Assert.Equal(array[1], value.P2);
			Assert.Equal(array[2], value.P3);
			Assert.Equal(array[3], value.P4);
			Assert.Equal(array[4], value.P5);
			Assert.Equal(array[5], value.P6);
			Assert.Equal(array[6], value.P7);
			Assert.Equal(array[7], value.P8);
			Assert.Equal(array[8], value.P9);
			Assert.Equal(array[9], value.P10);
		}

		[Theory]
		[InlineData("a invalid test case name")]
		public void TestInvalidParam1(string testCaseName) {
			Assert.Throws<ArgumentException>(() =>
			Assembly.GetExecutingAssembly().GetJsonTestCases<int>("Albatross.Repository.Test.TestCases.data.json", testCaseName));
		}
	}
}
