using System;

namespace Albatross.EFCore.Audit {
	public interface IModifiedUtc {
		public DateTime ModifiedUtc { get; set; }
	}
}
