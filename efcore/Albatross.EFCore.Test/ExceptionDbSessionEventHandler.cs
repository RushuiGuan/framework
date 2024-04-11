using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class ExceptionDbSessionEventHandler : IDbSessionEventHandler {
		public static bool ThrowPriorSaveException { get; set; }
		public override string ToString() => nameof(ExceptionDbSessionEventHandler);

		public Task PostSave() {
			throw new Exception("Test exception during post save");
		}

		public void OnAddedEntry(EntityEntry entry) {
			if(ThrowPriorSaveException) {
				throw new Exception("Test exception during pre save");
			}
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (ThrowPriorSaveException) {
				throw new Exception("Test exception during post save");
			}
		}

		public void OnDeletedEntry(EntityEntry entry) {
			if (ThrowPriorSaveException) {
				throw new Exception("Test exception during post save");
			}
		}

		public Task PreSave(IDbSession session) {
			if (ThrowPriorSaveException) {
				throw new Exception("Test exception during pre save");
			}
			return Task.CompletedTask;
		}
	}
}