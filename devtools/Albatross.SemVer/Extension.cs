using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.SemVer
{
    public static class Extension
    {
		public static StringBuilder Dot(this StringBuilder sb) {
			return sb.Append('.');
		}
		public static StringBuilder Plus(this StringBuilder sb) {
			return sb.Append('+');
		}
		public static StringBuilder Hyphen(this StringBuilder sb) {
			return sb.Append('-');
		}
	}
}
