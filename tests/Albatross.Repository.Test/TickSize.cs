using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albatross.Repository.Test {
	public record class TickSize : DateLevelEntity{
		public TickSize(int id, DateTime startDate, DateTime endDate, DateTime createdUtc, string createdBy, DateTime modifiedUtc, string modifiedBy) : base(id, startDate, endDate, createdUtc, createdBy, modifiedUtc, modifiedBy) {
		}
		public TickSize(int id, DateTime startDate, DateTime? endDate, decimal value, string user) : base(id, startDate, endDate, user){
			this.Value = value;
		}

		public FutureMarket Market { get; set; } = default!;
		[Precision(20, 10)]
		public decimal Value { get; set; }

		public override void Update(DateLevelEntity src) {
			throw new NotImplementedException();
		}
	}
	public class TickSizeEntityMap: DateLevelEntityEntityMap<TickSize> {
		public override void Map(EntityTypeBuilder<TickSize> entityTypeBuilder) {
			base.Map(entityTypeBuilder);
			entityTypeBuilder.HasOne(p => p.Market).WithMany(p => p.TickSizes).HasForeignKey(p => p.Id);
			entityTypeBuilder.Property(p => p.Id).HasColumnName("MarketId");
		}
	}
}