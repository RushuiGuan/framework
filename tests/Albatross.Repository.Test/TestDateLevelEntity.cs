using Albatross.Repository.Core;
using System;

namespace Albatross.Repository.Test {
	public class TestDateLevelEntity {
		public readonly static FutureMarket[] markets = new FutureMarket[] {
			new FutureMarket("C") {
				Id = 1,
				ContractSize = 500,
				TickSizes = {
					new TickSize(1, new DateTime(2022, 1, 1), DateLevelEntity.MaxEndDate, 12, "current-user"),
					new TickSize(1, new DateTime(2022, 2, 1), DateLevelEntity.MaxEndDate, 13, "current-user"),
					new TickSize(1, new DateTime(2022, 3, 1), DateLevelEntity.MaxEndDate, 14, "current-user"),
					new TickSize(1, new DateTime(2022, 4, 1), DateLevelEntity.MaxEndDate, 15, "current-user"),
				}
			},
			new FutureMarket("NG") {
				Id = 2,
				ContractSize = 1000,
				TickSizes = {
					new TickSize(1, new DateTime(2022, 1, 1), DateLevelEntity.MaxEndDate, 100, "current-user"),
					new TickSize(1, new DateTime(2022, 2, 1), DateLevelEntity.MaxEndDate, 200, "current-user"),
					new TickSize(1, new DateTime(2022, 3, 1), DateLevelEntity.MaxEndDate, 300, "current-user"),
					new TickSize(1, new DateTime(2022, 4, 1), DateLevelEntity.MaxEndDate, 400, "current-user"),
				}
			}
		};
	}
}
