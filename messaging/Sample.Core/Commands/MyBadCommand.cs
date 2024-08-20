﻿using Sample.Core.Commands.CodeGenTest;

namespace Sample.Core.Commands {
	public class MyBadCommand : IMyCommand {
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>
		///  this command cannot be deserialized property and will cause an error
		/// </summary>
		/// <param name="name"></param>
		/// <param name="myDescription"></param>
		public MyBadCommand(string name, string myDescription) {
			Name = name;
			Description = myDescription;
		}
	}
}
