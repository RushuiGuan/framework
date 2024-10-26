using System;

namespace Albatross.EFCore.ChangeReporting {
	[Flags]
	public enum ChangeType {
		Added = 1,
		Deleted = 2,
		Modified = 4,
		None = 0,
		All = Added | Deleted | Modified,
	}
}