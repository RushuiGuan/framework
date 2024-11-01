using System;
using System.Collections.Generic;

namespace Albatross.DateLevel.Test {
	public static class DateOnlyValues {
		public const string Any = "any";

		public static readonly DateOnly Jan1_2022 = new DateOnly(2022, 1, 1);
		public static readonly DateOnly Jan2_2022 = new DateOnly(2022, 1, 2);
		public static readonly DateOnly Jan31_2022 = new DateOnly(2022, 1, 31);

		public static readonly DateOnly Feb1_2022 = new DateOnly(2022, 2, 1);
		public static readonly DateOnly Feb2_2022 = new DateOnly(2022, 2, 2);
		public static readonly DateOnly Feb28_2022 = new DateOnly(2022, 2, 28);

		public static readonly DateOnly Mar1_2022 = new DateOnly(2022, 3, 1);
		public static readonly DateOnly Mar31_2022 = new DateOnly(2022, 3, 31);

		public static readonly DateOnly Apr1_2022 = new DateOnly(2022, 4, 1);
		public static readonly DateOnly Apr30_2022 = new DateOnly(2022, 4, 30);

		public static readonly DateOnly May1_2022 = new DateOnly(2022, 5, 1);
		public static readonly DateOnly May31_2022 = new DateOnly(2022, 5, 31);

		public static readonly DateOnly Jun1_2022 = new DateOnly(2022, 6, 1);
		public static readonly DateOnly Jun30_2022 = new DateOnly(2022, 6, 30);

		public static readonly DateOnly Jul1_2022 = new DateOnly(2022, 7, 1);
		public static readonly DateOnly Jul31_2022 = new DateOnly(2022, 7, 31);

		public static readonly DateOnly Aug1_2022 = new DateOnly(2022, 8, 1);
		public static readonly DateOnly Aug31_2022 = new DateOnly(2022, 8, 31);

		public static readonly DateOnly Sep1_2022 = new DateOnly(2022, 9, 1);
		public static readonly DateOnly Sep30_2022 = new DateOnly(2022, 9, 30);

		public static readonly DateOnly Oct1_2022 = new DateOnly(2022, 10, 1);
		public static readonly DateOnly Oct31_2022 = new DateOnly(2022, 10, 31);

		public static readonly DateOnly Nov1_2022 = new DateOnly(2022, 11, 1);
		public static readonly DateOnly Nov30_2022 = new DateOnly(2022, 11, 30);

		public static readonly DateOnly Dec1_2022 = new DateOnly(2022, 12, 1);
		public static readonly DateOnly Dec31_2022 = new DateOnly(2022, 12, 31);

		public static readonly DateOnly MaxSqlDate = new DateOnly(9999, 12, 31);
	}
}