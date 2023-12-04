using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Sample.Hosting.Utility {
	[Verb("hello-world")]
	public class HelloWorldOption: BaseOption {
	}
	public class HelloWorld : UtilityBase<HelloWorldOption> {
		public HelloWorld(HelloWorldOption option) : base(option) {
		}

		/// <summary>
		/// RunUtility is a method by convention.  The signature should always have a return type of Task&lt;int&gt;.  The parameters of the method can be any dependency 
		/// injected objects.  The parameters are created by DI container using a scoped lifetime.
		/// </summary>
		/// <param name="logger"></param>
		/// <returns>The return value of this method will set the exit code of the utility.  Normally return code of 0 indicates successful execution and another other values 
		/// are treated as an error.  However, return code is an old concept, best practise is to throw an exception for errors.
		/// to throw an exception </returns>
		public  Task<int> RunUtility(ILogger<HelloWorld> logger) {
			logger.LogInformation("Hello world at {datetime:yyyy-MM-dd hh:mm:ss fff}", DateTime.Now);
			return Task.FromResult(0);
		}
	}
}
