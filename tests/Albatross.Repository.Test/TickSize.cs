using Albatross.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Albatross.Repository.Test {
	public class TickSize : DateLevelEntity<int> {
		public int Id { get; set; }
		public TickSize(int marketId, DateTime startDate, decimal value) : base(startDate) {
			this.MarketId = marketId;
			this.Value = value;
		}

		public int MarketId { get; set; }
		public FutureMarket Market { get; set; } = default!;
		[Precision(20, 10)]
		public decimal Value { get; init; }

		public override int Key => MarketId;


		public override bool HasSameValue(DateLevelEntity src) {
			if (src is TickSize other) {
				return other.Value == this.Value;
			} else {
				return false;
			}
		}
	}
	public class TickSizeEntityMap : EntityMap<TickSize> { 
		public override void Map(EntityTypeBuilder<TickSize> builder) {
			base.Map(builder);
			builder.HasKey(p => p.Id).IsClustered(false);
			builder.HasIndex(p => new { p.MarketId, p.StartDate,}).IsUnique().IsClustered(true);
			builder.HasOne(p => p.Market).WithMany(p => p.TickSizes).HasForeignKey(p => p.MarketId);
		}
	}
}