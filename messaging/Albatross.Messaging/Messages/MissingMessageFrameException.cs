using System;

namespace Albatross.Messaging.Messages {
	public class MissingMessageFrameException : Exception {
		public MissingMessageFrameException(int actual, int expected)
			: base($"ZeroMQ message is missing required frames.  expected: {expected}, actual: {actual}") { }
	}
}