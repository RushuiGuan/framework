using Albatross.Repository.Core;
using Albatross.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Albatross.Repository.Test {
	public sealed record class TickSize : DateLevelEntity<int> {
		public TickSize(int marketId, DateTime startDate, decimal value) : base(startDate) {
			this.MarketId = marketId;
			this.Value = value;
		}

		public int MarketId { get; set; }
		public FutureMarket Market { get; set; } = default!;
		[Precision(20, 10)]
		public decimal Value { get; set; }

		public override int Key => MarketId;

		public override void Update(DateLevelEntity src) {
			if (src is TickSize newValue) {
				Value = newValue.Value;
			} else {
				throw new ArgumentException();
			}
		}

		public override bool HasSameValue(DateLevelEntity src) {
			if (src is TickSize other) {
				return other.Value == this.Value;
			} else {
				return false;
			}
		}
	}
	public class TickSizeEntityMap : EntityMap<TickSize> { 
		public override void Map(EntityTypeBuilder<TickSize> entityTypeBuilder) {
			base.Map(entityTypeBuilder);
			entityTypeBuilder.HasKey(p => new { p.MarketId, p.StartDate }).IsClustered(false);
			entityTypeBuilder.HasIndex(p => new { p.MarketId, p.StartDate, p.EndDate }).IsClustered(true);
			entityTypeBuilder.HasOne(p => p.Market).WithMany(p => p.TickSizes).HasForeignKey(p => p.MarketId);
		}
	}
}