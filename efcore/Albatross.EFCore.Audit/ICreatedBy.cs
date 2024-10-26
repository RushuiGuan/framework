using System;

namespace Albatross.EFCore.Audit {
	public interface ICreatedBy {
		public string CreatedBy { get; set; }
	}
}