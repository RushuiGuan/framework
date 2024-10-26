using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore {
	public class Data2 {
		public int Id { get; set; }
		public string Name { get; set; }
		public Data2(string name) {
			this.Name = name;
		}
		public JsonProperty? Property { get; set; } = new JsonProperty(null);
	}

	public class Data2EntityMap : EntityMap<Data2> {
		public override void Map(EntityTypeBuilder<Data2> builder) {
			builder.ToTable(nameof(Data2), b => b.IsTemporal());
			builder.HasKey(p => p.Id);
			builder.HasAlternateKey(p => p.Name);
			builder.Property(p => p.Property).HasImmutableJsonProperty();
		}
	}
}