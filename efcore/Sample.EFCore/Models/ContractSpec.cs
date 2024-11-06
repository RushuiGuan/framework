using Albatross.DateLevel;
using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore.Models {
	public class ContractSpec : DateLevelEntity<int> {
		public int Id { get; set; }
		public ContractSpec(int marketId, DateOnly startDate, decimal value) : base(startDate) {
			this.MarketId = marketId;
			this.Value = value;
		}

		public int MarketId { get; set; }
		public Market Market { get; set; } = default!;

		[Precision(20, 10)]
		public decimal Value { get; init; }

		public override int Key => MarketId;


		public override bool HasSameValue(DateLevelEntity src) {
			if (src is ContractSpec other) {
				return other.Value == this.Value;
			} else {
				return false;
			}
		}

		public override object Clone() {
			return new ContractSpec(MarketId, StartDate, Value) {
				EndDate = this.EndDate,
			};
		}
	}
	public class ContractSpecEntityMap : EntityMap<ContractSpec> {
		public override void Map(EntityTypeBuilder<ContractSpec> builder) {
			base.Map(builder);
			builder.HasKey(p => p.Id).IsClustered(false);
			builder.HasIndex(p => new { p.MarketId, p.StartDate, }).IsUnique().IsClustered(true);
			builder.HasOne(p => p.Market).WithMany(p => p.ContractSpec)
				.HasForeignKey(p => p.MarketId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

	// this should be excluded by codegen
	public class GenericEntityMap<T> : EntityMap<T> where T:class{
	}
}