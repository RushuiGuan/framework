using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.EFCore.CodeGen {
	public static class My {
		public const string EntityModelBuilderInterfaceName = "Albatross.EFCore.IBuildEntityModel";
		public const string DbSessionClassName = "Albatross.EFCore.DbSession";

		public static class Diagnostic {
			public const string IdPrefix = "EFCoreCodeGen";
		}
	}
}
