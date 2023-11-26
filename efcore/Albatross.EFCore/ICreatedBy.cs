using System;

namespace Albatross.EFCore {
	public interface ICreatedBy {
		public string CreatedBy { get; set; }
	}
	public interface IModifiedBy {
		public string ModifiedBy { get; set; }
	}
	public interface ICreatedUtc {
		public DateTime CreatedUtc { get; set; }
	}
	public interface IModifiedUtc {
		public DateTime ModifiedUtc { get; set; }
	}
}
