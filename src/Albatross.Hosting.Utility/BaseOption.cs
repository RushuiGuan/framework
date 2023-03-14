using Albatross.Logging;
using Albatross.Reflection;
using Albatross.Text;
using CommandLine;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Albatross.Hosting.Utility {
	public class BaseOption {
		[Option("console-out", HelpText = "The filename to save the console output")]
		public string? Output { get; set; }

		[Option("clipboard", HelpText ="Set this flag to copy the output to clipboard")]
		public bool Clipboard { get; set; }

		[Option("verbose", HelpText = "Set this flag to see logs at information level")]
		public bool Verbose{ get; set; }

		[Option("debug", HelpText = "Set this flag to see logs at debug level")]
		public bool Debug{ get; set; }


		public void ConfigureLogging(LoggerConfiguration cfg) {
			if (Debug) {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Debug);
			}else if (Verbose) {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Information);
			} else {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Error);
			}
		}

		void SendResult(string result) {
			Console.WriteLine(result);

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

		public void WriteProperties<T>(T? data, params string[] properties) {
			StringWriter writer = new StringWriter();
			writer.PrintProperties(data, properties);
			SendResult(writer.ToString());
		}

		public void WriteProperties<T>(IEnumerable<T> data, PrintPropertiesOption option) {
			StringWriter writer = new StringWriter();
			writer.PrintProperties<T>(data.ToArray(), option);
			SendResult(writer.ToString());
		}

		public void WriteTable<T>(IEnumerable<T> data, PrintTableOption option) {
			StringWriter writer = new StringWriter();
			writer.PrintTable<T>(data.ToArray(), option);
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

		public string? Prompt(string message) {
			Console.Write(message);
			return Console.ReadLine();
		}
	}
}