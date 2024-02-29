using System;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class ExceptionDbSessionEventHandler : IDbSessionEventHandler {
		public bool ThrowPriorSaveException { get; set; }

		public ExceptionDbSessionEventHandler(bool throwPriorSaveException) {
			ThrowPriorSaveException = throwPriorSaveException;
		}

		public Task PostSave() {
			throw new Exception("Test exception during post save");
		}

		public void PriorSave(IDbSession session) {
			if (ThrowPriorSaveException) {
				throw new Exception("Test exception during prior save");
			}
		}
	}
}