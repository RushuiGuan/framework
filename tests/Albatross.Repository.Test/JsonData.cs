using Albatross.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Albatross.EFCore.Test {
	public record class MyJsonData : IJsonData {
		public MyJsonData(string prefix, string algo, string broker) {
			this.Prefix = prefix;
			this.Algo = algo;
			this.Broker = broker;
		}
		public string Prefix { get; set; }
		public string Algo { get; set; }
		public string Broker { get; set; }
		public int Duration { get; set; }

		public string GetName(DateTime time) {
			return $"{Prefix}_{Algo}_{Broker}_Utc_Now_{time.AddSeconds(Duration):HHmm}";
		}
	}
	public record class HisJsonData : IJsonData {
		public string Prefix { get; set; }
		public string Algo { get; set; }
		public string Broker { get; set; }
		public HisJsonData(string prefix, string algo, string broker) {
			this.Prefix = prefix;
			this.Algo = algo;
			this.Broker = broker;
		}
		public bool HasExecutionWindow { get; set; }
		public bool UseCashClose { get; set; }
		public int ExecutionDuration { get; set; }

		public string GetName(DateTime triggerTime) {
			string result = $"{Prefix}_{Algo}_{Broker}";
			if (HasExecutionWindow) {
				string closing = UseCashClose ? "Close" : $"{triggerTime.AddSeconds(ExecutionDuration):HHmm}";
				return $"{result}_Utc_Now_{closing}";
			}
			return result;
		}
	}
	public record EmptyJsonData : IJsonData {
		public string GetName(DateTime triggerTime) => string.Empty;
	}

	[JsonDerivedType(typeof(MyJsonData), typeDiscriminator: "my")]
	[JsonDerivedType(typeof(HisJsonData), typeDiscriminator: "his")]
	[JsonDerivedType(typeof(EmptyJsonData), typeDiscriminator: "empty")]
	[JsonPolymorphic]
	public interface IJsonData {
		string GetName(DateTime triggerTime);
	}
	public class JsonData {
		public int Id { get; set; }
		public IJsonData Rule { get; set; } = null!;
	}

	public class JsonDataEntityMap : EntityMap<JsonData> {
		public override void Map(EntityTypeBuilder<JsonData> builder) {
			base.Map(builder);
			builder.HasKey(p => p.Id);
			builder.Property(p => p.Rule).HasConversion(this.GetJsonValueConverter<IJsonData>(() => new EmptyJsonData()));
		}
	}
}
