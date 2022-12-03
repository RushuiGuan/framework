using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albatross.Repository.Test {
	public sealed record class TickSize : DateLevelEntity{
		public TickSize(int id, DateTime startDate, DateTime endDate, DateTime createdUtc, string createdBy, DateTime modifiedUtc, string modifiedBy) : base(id, startDate, endDate, createdUtc, createdBy, modifiedUtc, modifiedBy) {
		}
		public TickSize(int id, DateTime startDate, DateTime? endDate, decimal value, string user) : base(id, startDate, endDate, user){
			this.Value = value;
		}

		public FutureMarket Market { get; set; } = default!;
		[Precision(20, 10)]
		public decimal Value { get; set; }

		public override void Update(DateLevelEntity src) {
			if(src is TickSize newValue) {
				Value = newValue.Value;
			} else {
				throw new ArgumentException();
			}
		}
		public override bool HasSameValue(DateLevelEntity src) {
			if(src is TickSize other) {
				return other.Value == this.Value;
			} else {
				return false;
			}
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