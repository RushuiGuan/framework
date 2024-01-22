using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albatross.EFCore.Test {
	public class FutureMarket2 {
		public int Id { get; set; }
		[MaxLength(Constant.NameLength)]
		public string Name { get; set; }

		public FutureMarket2(string name) {
			this.Name = name;
		}
		[Precision(20, 10)]
		public decimal ContractSize { get; set; }

		public List<TickSize2> TickSizes { get; set; } = new List<TickSize2>();

	}
	public class FutureMarket2EntityMap: EntityMap<FutureMarket2> {
		public override void Map(EntityTypeBuilder<FutureMarket2> builder) {
			base.Map(builder);
		}
	}
}