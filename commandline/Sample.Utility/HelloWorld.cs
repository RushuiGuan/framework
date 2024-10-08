using Albatross.Hosting.CommandLine;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Hosting.Utility {
	public enum MyEnum {
		One, Two, Three
	}


	//public partial class HelloWorldCommand : Command{
	//	public HelloWorldCommand() : base("hello-world", "Say hello to the world") {
			//this.AddOption(new Option<string>("--mu", "The mu value") {
			//	IsRequired = true,
			//	IsHidden = false,
			//	AllowMultipleArgumentsPerToken = false,
			//}.WithAlias("-m", "--m").FromAmong("a", "b", "c"));
//	}
//}


[Verb("hello-world")]
	public class HelloWorldOptions {
		[Option("mu", Required = true)]
		public string Mu { get; set; } = string.Empty;
	}
	public class HelloWorldCommandHandler : ICommandHandler{
		public HelloWorldCommandHandler(IOptions<HelloWorldOptions> option) { }
		
		public int Invoke(InvocationContext context)=> throw new NotSupportedException();

		public Task<int> InvokeAsync(InvocationContext context) {
			throw new NotImplementedException();
		}
	}
}
