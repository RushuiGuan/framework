using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("exception")]
	public class TestExceptionOption : MyBaseOption { }
	public class TestException : MyUtilityBase<TestExceptionOption> {
		public TestException(TestExceptionOption option) : base(option) {
		}
		public Task<int> RunUtility(ICommandClient client) {
			var myCmd = new TestExceptionCommand();
			client.Submit(myCmd);
			return Task.FromResult(0);
		}
	}
}
