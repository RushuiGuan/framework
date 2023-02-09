﻿using Castle.Components.DictionaryAdapter.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Repository.Test {
	public static class Constant {
		public const int NameLength = 256;
		public const string Schema = "test";

		public const string ConnectionString = "Server=.;Database=atlas;Trusted_Connection=true;Encrypt=No";
		public readonly static IEnumerable<FutureMarket> AllFutureMarkets = new FutureMarket[]{
			new FutureMarket("EC"),
			new FutureMarket("LL"),
			new FutureMarket("ST"),
			new FutureMarket("IR"),
			new FutureMarket("CF"),
			new FutureMarket("NG"),
			new FutureMarket("CL"),
			new FutureMarket("CO"),
			new FutureMarket("HO"),
			new FutureMarket("TY"),
			new FutureMarket("C "),
			new FutureMarket("QS"),
			new FutureMarket("HG"),
			new FutureMarket("XB"),
			new FutureMarket("EO"),
			new FutureMarket("XP"),
			new FutureMarket("XM"),
			new FutureMarket("YM"),
			new FutureMarket("FV"),
			new FutureMarket("US"),
			new FutureMarket("TU"),
			new FutureMarket("VG"),
			new FutureMarket("GX"),
			new FutureMarket("RX"),
			new FutureMarket("OE"),
			new FutureMarket("DU"),
			new FutureMarket("UB"),
			new FutureMarket("HC"),
			new FutureMarket("HI"),
			new FutureMarket("TP"),
			new FutureMarket("KW"),
			new FutureMarket("ER"),
			new FutureMarket("Z "),
			new FutureMarket("G "),
			new FutureMarket("L "),
			new FutureMarket("SI"),
			new FutureMarket("CC"),
			new FutureMarket("KC"),
			new FutureMarket("SB"),
			new FutureMarket("CT"),
			new FutureMarket("GC"),
			new FutureMarket("NQ"),
			new FutureMarket("ES"),
			new FutureMarket("IH"),
			new FutureMarket("PT"),
			new FutureMarket("CN"),
			new FutureMarket("IB"),
			new FutureMarket("QC"),
			new FutureMarket("LA"),
			new FutureMarket("LP"),
			new FutureMarket("LN"),
			new FutureMarket("LX"),
			new FutureMarket("S "),
			new FutureMarket("BO"),
			new FutureMarket("W "),
			new FutureMarket("SM"),
			new FutureMarket("LC"),
			new FutureMarket("LH"),
			new FutureMarket("FC"),
			new FutureMarket("BJ"),
			new FutureMarket("NI"),
			new FutureMarket("PE"),
			new FutureMarket("JB"),
			new FutureMarket("CD"),
			new FutureMarket("BP"),
			new FutureMarket("AD"),
			new FutureMarket("ED"),
			new FutureMarket("SF"),
			new FutureMarket("RY"),
			new FutureMarket("JY"),
			new FutureMarket("NV"),
			new FutureMarket("DM"),
			new FutureMarket("DF"),
			new FutureMarket("UX"),
			new FutureMarket("DED"),
			new FutureMarket("MO"),
			new FutureMarket("IK"),
			new FutureMarket("MES"),
			new FutureMarket("OAT"),
			new FutureMarket("QZ"),
			new FutureMarket("RTY"),
			new FutureMarket("SFR"),
			new FutureMarket("SFI"),
		};
	}
}
