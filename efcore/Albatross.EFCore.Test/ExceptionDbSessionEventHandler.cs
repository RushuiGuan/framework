using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class PostSaveException : Exception { }
	public class PreSaveException : Exception { }
	public class ExceptionDbSessionEventHandler : IDbSessionEventHandler {
		public static bool ThrowPriorSaveException { get; set; }
		public override string ToString() => nameof(ExceptionDbSessionEventHandler);

		public Task PostSave() {
			throw new PostSaveException();
		}

		public void OnAddedEntry(EntityEntry entry) {
			if (ThrowPriorSaveException) {
				throw new PreSaveException();
			}
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (ThrowPriorSaveException) {
				throw new PreSaveException();
			}
		}

		public void OnDeletedEntry(EntityEntry entry) {
			if (ThrowPriorSaveException) {
				throw new PreSaveException();
			}
		}

		public Task PreSave(IDbSession session) {
			if (ThrowPriorSaveException) {
				throw new PreSaveException();
			}
			return Task.CompletedTask;
		}
	}
}