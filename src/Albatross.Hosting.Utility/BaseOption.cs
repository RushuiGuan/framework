using CommandLine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Albatross.Hosting.Utility {
	public class BaseOption {
		[Option('o', "out", Required = false, HelpText = "Output file name")]
		public string Output { get; set; }

		[Option('v', "verbose")]
		public bool Verbose { get; set; }

		[Option('c', "clipboard")]
		public bool Clipboard { get; set; }


		void SendResult(string result) {
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

		public void WriteOutput(object data) {
			string result;
			if (data is string) {
				result = (string)data;
			} else {
				var jsonOption = new JsonSerializerOptions {
					IgnoreNullValues = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					WriteIndented = true,
				};
				result = JsonSerializer.Serialize(data, data.GetType(), jsonOption);
			}
			SendResult(result);
		}
		public void WriteCsvOutput<T>(IEnumerable<T> items) {
			StringBuilder sb = new StringBuilder();
			using (StringWriter writer = new StringWriter(sb)) {
				using (var csvWriter = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture)) {
					csvWriter.WriteHeader<T>();
					writer.WriteLine();
					csvWriter.WriteRecords<T>(items);
				}
			}
			SendResult(sb.ToString());
		}
	}
}
