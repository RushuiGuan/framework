using System;

namespace Albatross.Messaging.Messages {
	public class InvalidMsgLogException : Exception {
		public InvalidMsgLogException(string line)
			: base($"Message log has bad format and cannot be parsed\n{line}") {
		}
	}
}