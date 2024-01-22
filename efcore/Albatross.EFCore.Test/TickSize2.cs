using Albatross.DateLevel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Albatross.EFCore.Test {
	public class TickSize2 : DateLevelEntity<int> {
		public int Id { get; set; }
		public TickSize2(int marketId, DateOnly startDate, decimal value) : base(startDate) {
			this.MarketId = marketId;
			this.Value = value;
		}

		public int MarketId { get; set; }
		public FutureMarket2 Market { get; set; } = default!;
		[Precision(20, 10)]
		public decimal Value { get; init; }

		public override int Key => MarketId;


		public override bool HasSameValue(DateLevelEntity src) {
			if (src is TickSize2 other) {
				return other.Value == this.Value;
			} else {
				return false;
			}
		}
	}
	public class TickSize2EntityMap : EntityMap<TickSize2> {
		public override void Map(EntityTypeBuilder<TickSize2> builder) {
			base.Map(builder);
			builder.HasKey(p => p.Id).IsClustered(false);
			builder.HasIndex(p => new { p.MarketId, p.StartDate, }).IsUnique().IsClustered(true);
			builder.HasOne(p => p.Market).WithMany(p => p.TickSizes).HasForeignKey(p => p.MarketId);
		}
	}
}