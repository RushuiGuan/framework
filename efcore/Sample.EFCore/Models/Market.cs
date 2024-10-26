using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Sample.EFCore.Models {
	public class Market {
		public int Id { get; set; }

		[Unicode(false)]
		[MaxLength(My.NameLength)]
		public string Name { get; set; }

		public Market(string name) {
			this.Name = name;
		}


		public List<ContractSpec> ContractSpec { get; set; } = new List<ContractSpec>();
		public List<SpreadSpec> SpreadSpec { get; set; } = new List<SpreadSpec>();

	}
	public class MarketEntityMap : EntityMap<Market> {
		public override void Map(EntityTypeBuilder<Market> builder) {
			base.Map(builder);
			builder.HasKey(i => i.Id);
		}
	}
}