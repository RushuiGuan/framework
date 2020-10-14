using Albatross.Config.Core;
using Microsoft.Extensions.Logging;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Albatross.Hosting {
	public interface ITransformAngularConfig {
		void Transform();
	}
	public class TransformAngularConfig : ITransformAngularConfig {
		private readonly AngularConfig config;
		private readonly ILogger<TransformAngularConfig> logger;

		public TransformAngularConfig(AngularConfig config, ProgramSetting programSetting, ILogger<TransformAngularConfig> logger) {
			this.config = config;
			this.logger = logger;
		}

		public void Transform() {
			var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
			if (config.Location?.Length > 0 && config.Transformations?.Length > 0 && !string.IsNullOrEmpty(env)) {
				foreach(var configFile in config.Transformations) {
					string file = GetConfigFile(configFile, null);
					string change = GetConfigFile(configFile, env);
					if (!File.Exists(file)) {
						logger.LogError("Angular config file {file} doesn't exist", file);
					}else if (!File.Exists(change)) {
						logger.LogError("Angular config transformation file {file} doesn't exist", change);
					} else {
						var srcElem = ReadJsonFile(file);
						var changeElem = ReadJsonFile(change);
						var result = Albatross.Serialization.Extension.ApplyJsonValue(srcElem, changeElem);
						logger.LogInformation("Overriding config file {config} with values from {environment_config}", configFile, Path.GetFileName(change));
						WriteJsonFile(file, result);
					}
				}
			}
			UpdateBaseHref();
		}

		public void UpdateBaseHref() {
			if (config.BaseHrefFile?.Length > 0) {
				var indexHtml = Path.Join((new string[] { System.Environment.CurrentDirectory }.Union(config.Location).Union(config.BaseHrefFile)).ToArray());
				if (File.Exists(indexHtml)) {
					logger.LogInformation("Replacing baseHref for {file}; the framework will only find and replace the exact string: {target}", indexHtml, "<base href=\"/\">");
					string content;
					using(var reader = new StreamReader(indexHtml)) {
						content = reader.ReadToEnd();
						string pattern = "<\\s*base\\s+href\\s*=\\s*\"[a-z0-9_\\-/]*\"\\s*>";
						Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
						string replacement = $"<base href=\"{config.BaseHref}\">";
						content = regex.Replace(content, replacement);
					}
					using(var writer = new StreamWriter(indexHtml)) {
						writer.Write(content);
					}
				} else {
					logger.LogError("Angular index html file {name} doesn't exist", indexHtml);
				}
			} else {
				logger.LogWarning("Angular IndexHtml path not specified, baseHref transformation skipped");
			}
		}
		string GetConfigFile(string name, string environment) {
			if (!string.IsNullOrEmpty(environment)) {
				name = $"{Path.GetFileNameWithoutExtension(name)}.{environment}.json";
			}
			var location = new string[] {
				System.Environment.CurrentDirectory
			}.Union(config.Location).Union(new string[] { name}).ToArray();
			return Path.Join(location);
		}

		JsonElement ReadJsonFile(string file) {
			using(var reader = new StreamReader(file)) {
				string text = reader.ReadToEnd();
				return JsonSerializer.Deserialize<JsonElement>(text);
			}
		}
		void WriteJsonFile(string file, JsonElement element) {
			string content = JsonSerializer.Serialize<JsonElement>(element, new JsonSerializerOptions { WriteIndented=true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  });
			using(StreamWriter writer = new StreamWriter(file)) {
				writer.Write(content);
			}
		}
	}
}
