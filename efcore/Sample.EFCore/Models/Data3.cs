using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore {
	public class Data3 {
		public int Id { get; set; }
		public string Name { get; set; }
		public Data3(string name) {
			this.Name = name;
		}
		public List<JsonProperty> ArrayProperty { get; set; } = new List<JsonProperty>();
	}

	public class Data3EntityMap : EntityMap<Data3> {
		public override void Map(EntityTypeBuilder<Data3> builder) {
			builder.ToTable(nameof(Data3), b => b.IsTemporal());
			builder.HasKey(p => p.Id);
			builder.HasAlternateKey(p => p.Name);
			builder.Property(p => p.ArrayProperty).HasJsonCollectionProperty();
		}
	}
}