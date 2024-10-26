using Albatross.Caching;
using Albatross.EFCore;
using Albatross.EFCore.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore {
	public class MyDataCacheKey : CacheKey {
		public MyDataCacheKey(int id) : base("my-data", id.ToString(), true) { }
	}
	public class MyData : IModifiedBy, IModifiedUtc, ICreatedBy, ICreatedUtc, ICachedObject<DbContext, PropertyEntry> {
		public int Id { get; set; }
		public JsonProperty? Property { get; set; }
		public List<JsonProperty> ArrayProperty { get; set; } = new List<JsonProperty>();

		[Unicode(false)]
		public string? Text { get; set; }
		public DateOnly Date { get; set; }
		public DateTime DateTime { get; set; }
		public DateTime UtcTimeStamp { get; set; }
		public int Int { get; set; }
		[Precision(18, 6)]
		public decimal Decimal { get; set; }
		public bool Bool { get; set; }
		public double Double { get; set; }
		public float Float { get; set; }
		public Guid Guid { get; set; }


		public string ModifiedBy { get; set; } = string.Empty;
		public DateTime ModifiedUtc { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
		public DateTime CreatedUtc { get; set; }

		public IEnumerable<ICacheKey> CreateCacheKeys(ObjectState state, DbContext context, IEnumerable<PropertyEntry> changes) {
			return [new MyDataCacheKey(Id)];
		}
	}

	public class JsonDataEntityMap : EntityMap<MyData> {
		public override void Map(EntityTypeBuilder<MyData> builder) {
			builder.HasKey(p => p.Id);
			base.Map(builder);
			builder.Property(p => p.Property).HasImmutableJsonProperty();
			builder.Property(p => p.ArrayProperty).HasJsonCollectionProperty();
		}
	}
}