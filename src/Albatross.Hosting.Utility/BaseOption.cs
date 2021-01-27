using CommandLine;
using System;
using System.IO;
using System.Text.Json;

namespace Albatross.Hosting.Utility {
	public class BaseOption {
		[Option('o', "out", Required = false, HelpText = "Output file name")]
		public string Output { get; set; }

		[Option('v', "verbose")]
		public bool Verbose { get; set; }

		[Option('c', "clipboard")]
		public bool Clipboard { get; set; }


		public void WriteOutput(object data) {
			var jsonOption = new JsonSerializerOptions {
				IgnoreNullValues = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true,
			};
			string result = JsonSerializer.Serialize(data, data.GetType(), jsonOption);

			if (Verbose) { Console.WriteLine(result); }

			if (!string.IsNullOrEmpty(Output)) {
				using var stream = System.IO.File.OpenWrite(Output);
				using var fileWriter = new StreamWriter(stream);
				fileWriter.Write(result);
				fileWriter.Flush();
				stream.SetLength(stream.Position);
			}

			if (Clipboard) {
				new TextCopy.Clipboard().SetText(result);
			}
		}
	}
}
