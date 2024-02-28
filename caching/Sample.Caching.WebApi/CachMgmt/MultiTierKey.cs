namespace Sample.Caching.WebApi {
	public record class MultiTierKey {
		public MultiTierKey() : this(0, 0, 0) { }
		public MultiTierKey(int tier1) : this(tier1, 0, 0) { }
		public MultiTierKey(int tier1, int tier2) : this(tier1, tier2, 0) { }
		public MultiTierKey(int tier1, int tier2, int tier3) {
			Tier1 = tier1;
			Tier2 = tier2;
			Tier3 = tier3;
		}

		public int Tier1 { get; set; }
		public int Tier2 { get; set; }
		public int Tier3 { get; set; }

		public int Data => Tier1 + Tier2 + Tier3;
	}
}


// 1:2:3:
// 1:4:5:
// 2:1:1