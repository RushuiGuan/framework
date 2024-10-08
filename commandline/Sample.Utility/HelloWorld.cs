using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Sample.Hosting.Utility {
	public enum MyEnum {
		One, Two, Three
	}

	[Verb("hello-world")]
	public class HelloWorldOption : BaseOption {
		[Option("mu", Required = true)]
		public string Mu { get; set; } = string.Empty;
	}
	public class HelloWorld : UtilityBase<HelloWorldOption> {
		public HelloWorld(HelloWorldOption option) : base(option) { }

		/// <summary>
		/// RunUtility is a method by convention.  The signature should always have a return type of Task&lt;int&gt;.  The parameters of the method can be any dependency 
		/// injected objects.  The parameters are created by DI container using a scoped lifetime.
		/// </summary>
		/// <param name="logger"></param>
		/// <returns>The return value of this method will set the exit code of the utility.  Normally return code of 0 indicates successful execution and another other values 
		/// are treated as an error.  However, return code is an old concept, best practise is to throw an exception for errors.
		/// to throw an exception </returns>
		public Task<int> RunUtility(ILogger<HelloWorld> logger) {
			var myEnum = this.Options.ParseEnum<MyEnum>(Options.Mu);
			logger.LogInformation("Hello world at {datetime:yyyy-MM-dd hh:mm:ss fff}", DateTime.Now);
			throw new InvalidOperationException("test");
		}
	}
}
