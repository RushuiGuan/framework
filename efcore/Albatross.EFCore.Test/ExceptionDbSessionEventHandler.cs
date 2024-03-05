using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class ExceptionDbSessionEventHandler : IDbChangeEventHandler {
		public bool ThrowPriorSaveException { get; set; }

		public bool HasPostSaveOperation => true;

		public ExceptionDbSessionEventHandler(bool throwPriorSaveException) {
			ThrowPriorSaveException = throwPriorSaveException;
		}

		public Task PostSave() {
			throw new Exception("Test exception during post save");
		}

		public void OnAddedEntry(EntityEntry entry) {
			throw new Exception("Test exception during post save");
		}

		public void OnModifiedEntry(EntityEntry entry) {
			throw new Exception("Test exception during post save");
		}

		public void OnDeletedEntry(EntityEntry entry) {
			throw new Exception("Test exception during post save");
		}
	}
}