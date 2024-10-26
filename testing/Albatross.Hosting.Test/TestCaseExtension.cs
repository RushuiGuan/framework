using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Hosting.Test {
	public static class TestCaseExtension {
		static JsonSerializerOptions defaultJsonSerializerOption = new JsonSerializerOptions {
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true,
		};

		public static void CreateJsonTestCases<T>(this T[] data, string filename) where T : TestCase {
			using var stream = File.OpenWrite(filename);
			JsonSerializer.Serialize<T[]>(stream, data, defaultJsonSerializerOption);
		}

		static T _GetJsonTestCase<T>(this Assembly assembly, string embeddedFile, string name) where T : TestCase {
			using var stream = assembly.GetManifestResourceStream(embeddedFile);
			if (stream == null) {
				throw new ArgumentException("Invalid embedded file path");
			}
			var items = JsonSerializer.Deserialize<T[]>(stream, defaultJsonSerializerOption);
			var result = items?.Where(args => args.Name == name)?.FirstOrDefault();
			if (result == null) {
				throw new ArgumentException($"Test case {name} was not found in embedded file {embeddedFile}");
			}
			return result;
		}

		public static TestCase<T> GetJsonTestCases<T>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T>>(embeddedFile, name);

		public static TestCase<T1, T2> GetJsonTestCases<T1, T2>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2>>(embeddedFile, name);

		public static TestCase<T1, T2, T3> GetJsonTestCases<T1, T2, T3>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4> GetJsonTestCases<T1, T2, T3, T4>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4, T5> GetJsonTestCases<T1, T2, T3, T4, T5>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4, T5>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4, T5, T6> GetJsonTestCases<T1, T2, T3, T4, T5, T6>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4, T5, T6>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4, T5, T6, T7> GetJsonTestCases<T1, T2, T3, T4, T5, T6, T7>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4, T5, T6, T7>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4, T5, T6, T7, T8> GetJsonTestCases<T1, T2, T3, T4, T5, T6, T7, T8>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4, T5, T6, T7, T8>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4, T5, T6, T7, T8, T9> GetJsonTestCases<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(embeddedFile, name);

		public static TestCase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetJsonTestCases<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Assembly assembly, string embeddedFile, string name)
			=> assembly._GetJsonTestCase<TestCase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(embeddedFile, name);
	}
}