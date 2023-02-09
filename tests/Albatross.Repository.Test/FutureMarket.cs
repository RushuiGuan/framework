using Albatross.Repository.Core;
using Albatross.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Test {
	public class FutureMarket {
		public int Id { get; set; }
		[MaxLength(Constant.NameLength)]
		public string Name { get; set; }

		public FutureMarket(string name) {
			this.Name = name;
		}
		[Precision(20, 10)]
		public decimal ContractSize { get; set; }

		public List<TickSize> TickSizes { get; set; } = new List<TickSize>();

	}
	public class FutureMarketEntityMap: EntityMap<FutureMarket> {
		public override void Map(EntityTypeBuilder<FutureMarket> builder) {
			base.Map(builder);
		}
	}
}