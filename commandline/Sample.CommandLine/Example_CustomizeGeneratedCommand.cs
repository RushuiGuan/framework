using Albatross.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("customized-generated-command", Description = "This command is generated and customized")]
	public record class CustomizedGeneratedCommandOptions {
		public string Value { get; set; } = string.Empty;
	}
	public partial class CustomizedGeneratedCommand : IRequireInitialization {
		public void Init() {
			this.Option_Value.SetDefaultValue("test");
		}
	}
}
