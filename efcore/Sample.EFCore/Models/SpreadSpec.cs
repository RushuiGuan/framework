using Albatross.DateLevel;
using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore.Models {
	public class SpreadSpec : DateLevelEntity<int> {
		public SpreadSpec(int marketId, DateOnly startDate, decimal value) : base(startDate) {
			this.MarketId = marketId;
			this.Value = value;
		}

		public int Id { get; set; }
		public int MarketId { get; set; }
		public Market Market { get; set; } = default!;

		[Precision(20, 10)]
		public decimal Value { get; init; }

		public override int Key => MarketId;


		public override bool HasSameValue(DateLevelEntity src) {
			if (src is SpreadSpec other) {
				return other.Value == this.Value;
			} else {
				return false;
			}
		}
		public override object Clone() {
			return new SpreadSpec(MarketId, StartDate, Value) {
				EndDate = this.EndDate,
			};
		}
	}
	public class SpreadSpecEntityMap : EntityMap<SpreadSpec> {
		public override void Map(EntityTypeBuilder<SpreadSpec> builder) {
			base.Map(builder);
			builder.HasKey(p => p.Id).IsClustered(false);
			builder.HasIndex(p => new { p.MarketId, p.StartDate, }).IsUnique().IsClustered(true);
			builder.HasOne(p => p.Market).WithMany(p => p.SpreadSpec)
				.HasForeignKey(p => p.MarketId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}