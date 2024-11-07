using Albatross.Logging;
using Albatross.Text;
using CommandLine;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Hosting.Utility {
	public class BaseOption {
		[Option("console-out", HelpText = "The filename to save the console output")]
		public string? Output { get; set; }

		[Option("clipboard", HelpText = "Set this flag to copy the output to clipboard")]
		public bool Clipboard { get; set; }

		[Option("verbose", HelpText = "Set this flag to see logs at verbose level")]
		public bool Verbose { get; set; }

		[Option("information", HelpText = "Set this flag to see logs at information level")]
		public bool Information { get; set; }

		[Option("warning", HelpText = "Set this flag to see logs at warning level")]
		public bool Warning { get; set; }

		[Option("debug", HelpText = "Set this flag to see logs at debug level")]
		public bool Debug { get; set; }

		[Option("benchmark", HelpText = "Use this flag to measure the total amount of execution time")]
		public bool Benchmark { get; set; }

		public void ConfigureLogging(LoggerConfiguration cfg) {
			if (Debug) {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Debug);
			} else if (Verbose) {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Verbose);
			} else if (Information) {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Information);
			} else if (Warning) {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Warning);
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

		public async Task WriteProperties<T>(T? data, PrintPropertiesOption option) {
			StringWriter writer = new StringWriter();
			await writer.PrintProperties(data, option);
			SendResult(writer.ToString());
		}

		public async Task WriteArrayProperties<T>(IEnumerable<T> data, PrintPropertiesOption option) {
			StringWriter writer = new StringWriter();
			await writer.PrintProperties<T>(data.ToArray(), option);
			SendResult(writer.ToString());
		}

		public async Task WriteTable<T>(IEnumerable<T> data, PrintTableOption option) {
			StringWriter writer = new StringWriter();
			await writer.PrintTable<T>(data.ToArray(), option);
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

		public static string GetEnumHelpText<T>(string description = "") where T : struct, Enum {
			var writer = new StringWriter();
			if (!string.IsNullOrEmpty(description)) {
				writer.Append(description).Append(". ");
			}
			writer.Append("The available values are ").WriteItems<T>(Enum.GetValues<T>(), ", ");
			return writer.ToString();
		}

		public T ParseEnum<T>(string? value, T? defaultValue = null) where T : struct, Enum {
			if (string.IsNullOrEmpty(value)) {
				return defaultValue ?? throw new ArgumentException(GetEnumHelpText<T>($"No value provided for {typeof(T).Name}"));
			} else if (Enum.TryParse<T>(value, true, out T result)) {
				return result;
			} else {
				throw new ArgumentException(GetEnumHelpText<T>($"Invalid value '{value}' for {typeof(T).Name}"));
			}
		}
	}
}