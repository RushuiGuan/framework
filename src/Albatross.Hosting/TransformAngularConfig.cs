using Albatross.Config.Core;
using Microsoft.Extensions.Logging;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public interface ITransformAngularConfig {
		void Transform();
	}
	public class TransformAngularConfig : ITransformAngularConfig {
		private readonly AngularConfig config;
		private readonly ProgramSetting programSetting;
		private readonly ILogger<TransformAngularConfig> logger;

		public TransformAngularConfig(AngularConfig config, ProgramSetting programSetting, ILogger<TransformAngularConfig> logger) {
			this.config = config;
			this.programSetting = programSetting;
			this.logger = logger;
		}

		public void Transform() {
			if(config.Transformations?.Length > 0) {
				foreach(var configFile in config.Transformations) {
					string file = GetConfigFile(configFile, null);
					string change = GetConfigFile(configFile, programSetting.Environment);
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
