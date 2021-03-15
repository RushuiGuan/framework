using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace Albatross.Repository.TimeSeries.Test {
	public class FundEquityDto : TimeSeriesDto {
		public int FundId { get; set; }
		public DateTime EffectiveDate { get; set; }
		public decimal Nav { get; set; }
	}

	public class FundEquity : TimeSeriesEntity<FundEquity, FundEquityDto> {
		public int FundId { get; private set; }
		public DateTime EffectiveDate { get; private set; }
		public decimal Nav { get; private set; }
		public override Expression<Func<FundEquity, bool>> SeriesExpression => (args) => args.FundId == FundId && args.EffectiveDate == EffectiveDate;

		public FundEquity() { }
		public FundEquity(FundEquityDto src, string user, IDbSession session) {
			Create(src, user, session);
		}

		public override FundEquity Clone(string user) {
			FundEquity value = base.Clone(user);
			value.FundId = FundId;
			value.EffectiveDate = EffectiveDate;
			value.Nav = Nav;
			return value;
		}
		protected override void Create(FundEquityDto src, string user, IDbSession session) {
			Nav = src.Nav;
			EffectiveDate = src.EffectiveDate;
			FundId = src.FundId;
			base.Create(src, user, session);
		}

		public override bool CanMerge(FundEquity item) {
			return FundId == item.FundId && EffectiveDate == item.EffectiveDate && Nav == item.Nav;
		}
		public FundEquityDto CreateDto() {
			return new FundEquityDto {
				FundId = FundId,
				EffectiveDate = EffectiveDate,
				Nav = Nav,
				Start = Start,
				End = End,
				Id = Id,
			};
		}
	}



	public class FundEquityEntityMap : ImmutableEntityMap<FundEquity> {
		public override void Map(EntityTypeBuilder<FundEquity> builder) {
			base.Map(builder);
			builder.HasKey(p => p.Id).IsClustered(false);
			builder.HasIndex(p => new { p.FundId, p.EffectiveDate, p.Start, p.End }).IsClustered(true).IsUnique(true);
			builder.Property(p => p.Nav).HasPrecision(20, 4);
		}
	}
}
