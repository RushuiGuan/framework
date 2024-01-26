using Albatross.EFCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore {
	public class MyData {
		public int Id { get; set; }
		public JsonProperty Property { get; set; } = new JsonProperty(string.Empty);
	}

	public record class JsonProperty{
		public string Text { get; set; }
		public JsonProperty(string text) {
			Text = text;
		}
	}

	public class JsonDataEntityMap : EntityMap<MyData> {
		public override void Map(EntityTypeBuilder<MyData> builder) {
			builder.HasKey(p => p.Id);
			base.Map(builder);
			builder.Property(p => p.Property).HasJsonProperty(()=>new JsonProperty(string.Empty));
		}
	}
}
