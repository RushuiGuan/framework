using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore {
	public class MyData {
		public int Id { get; set; }
		[Unicode(false)]
		public string? Text { get; set; }
		public JsonProperty Property { get; set; } = new JsonProperty(null);
		public List<JsonProperty> ArrayProperty { get; set; } = new List<JsonProperty>();
	}

	public record class JsonProperty: IJsonProperty<JsonProperty>{
		public string? Text { get; set; }
		public JsonProperty(string? text) {
			Text = text;
		}
		public JsonProperty Snapshot() => new JsonProperty(Text);
	}

	public class JsonDataEntityMap : EntityMap<MyData> {
		public override void Map(EntityTypeBuilder<MyData> builder) {
			builder.HasKey(p => p.Id);
			base.Map(builder);
			builder.Property(p => p.Property).HasJsonProperty(()=> new JsonProperty(null));
			builder.Property(p => p.ArrayProperty).HasJsonCollectionProperty();
		}
	}
}
