using System;

namespace Albatross.Messaging.Commands {
	public class CommandAttribute : Attribute {
		public Type ResponseType { get; init; }
		public CommandAttribute() { 
			ResponseType = typeof(void); 
		}
		public CommandAttribute(Type responseType) {
			ResponseType = responseType;
		}
	}
}
