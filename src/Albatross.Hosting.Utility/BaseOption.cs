using Albatross.Reflection;
using Albatross.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Albatross.Hosting.Utility {
	public class BaseOption {
		[Option('o', "console-out", Required = false, HelpText = "Console output file name")]
		public string? LogFile { get; set; }

		[Option('c', "clipboard")]
		public bool Clipboard { get; set; }


		void SendResult(string result) {
			Console.WriteLine(result);

			if (!string.IsNullOrEmpty(LogFile)) {
				using var stream = System.IO.File.OpenWrite(LogFile);
				using var fileWriter = new StreamWriter(stream);
				fileWriter.Write(result);
				fileWriter.Flush();
				stream.SetLength(stream.Position);
			}

			if (Clipboard) {
				new TextCopy.Clipboard().SetText(result);
			}
		}

		public void WriteProperties<T>(T? data, params string[] properties) {
			StringWriter writer = new StringWriter();
			writer.PrintProperties(data, properties);
			SendResult(writer.ToString());
		}

		public void WriteProperties<T>(IEnumerable<T> data, PrintPropertiesOption option, params string[] properties) {
			StringWriter writer = new StringWriter();
			writer.PrintProperties<T>(data.ToArray(), option,  properties);
			SendResult(writer.ToString());
		}

		public void WriteTable<T>(IEnumerable<T> data, PrintTableOption option, params string[] properties) {
			StringWriter writer = new StringWriter();
			writer.PrintTable<T>(data.ToArray(), option, properties);
			SendResult(writer.ToString());
		}

		public void WriteOutput(object data) {
			string result;
			if (data is string) {
				result = (string)data;
			} else {
				var jsonOption = new JsonSerializerOptions {
					DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
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
					csvWriter.NextRecord();
					csvWriter.WriteRecords<T>(items);
				}
			}
			SendResult(sb.ToString());
		}
	}
}