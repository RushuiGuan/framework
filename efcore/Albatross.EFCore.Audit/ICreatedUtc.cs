using System;

namespace Albatross.EFCore.Audit {
	public interface ICreatedUtc {
		public DateTime CreatedUtc { get; set; }
	}
}
