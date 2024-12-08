﻿using Albatross.CommandLine;

namespace Sample.CommandLine {
	[Verb("customized-generated-command", Description = "This command is generated and customized")]
	public record class CustomizedGeneratedCommandOptions {
		public string Value { get; set; } = string.Empty;

		[Option(DefaultToInitializer = true)]
		public int Number { get; set; } = 100;
	}
	/// <summary>
	/// To customized a generated command.  Declare a partial class with the same name as the generated command
	/// The class should implement IRequireInitialization.  The Init method will be called after the command instance is created.
	/// This is done since the constructor is being generated and cannot be accessed by the partial class.
	/// </summary>
	public partial class CustomizedGeneratedCommand : IRequireInitialization {
		public void Init() {
			this.Option_Value.SetDefaultValue("test");
			this.Handler = new DefaultCommandHandler();
		}
	}
}
