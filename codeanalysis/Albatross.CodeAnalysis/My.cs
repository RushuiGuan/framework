using System.IO;

namespace Albatross.CodeAnalysis {
	public static class My {
		public static class GenericDefinition {
			public const string Nullable = "System.Nullable<>";
			public const string IEnumerable = "System.Collections.Generic.IEnumerable<>";
			public const string IAsyncEnumerable = "System.Collections.Generic.IAsyncEnumerable<>";
			public const string List = "System.Collections.Generic.List<>";
			public const string ICollection = "System.Collections.Generic.ICollection<>";
		}
		public static class Class {
			public const string IEnumerable = "System.Collections.IEnumerable";
			public const string CodeGenExtensions = "CodeGenExtensions";
		}
		public static class Namespace {
			public const string System = "System";
			public const string System_Text_Json_Serialization = "System.Text.Json.Serialization";
			public const string System_IO = "System.IO";
			public const string System_Threading_Tasks = "System.Threading.Tasks";
			public const string Microsoft_Extensions_DependencyInjection = "Microsoft.Extensions.DependencyInjection";
			public const string System_Collections_Generic = "System.Collections.Generic";
			public const string Microsoft_EntityFrameworkCore = "Microsoft.EntityFrameworkCore";
		}

		public static TextWriter WriteSourceHeader(this TextWriter writer, string filename) {
			writer.WriteLine($"// ********** {filename}.cs ********** //");
			return writer;
		}
	}
}