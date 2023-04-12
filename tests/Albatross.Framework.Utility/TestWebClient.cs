using Albatross.Hosting.Utility;
using CommandLine;
using System;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	[Verb("test-httpclient-proxy")]
	public class TestWebClientCreateRequestOption : BaseOption {
		[Option('i')]
		public int TestId { get; set; }

		[Option('r')]
		public bool LogRawData { get; set; }
	}

	public class TestWebClientCreateRequest : MyUtilityBase<TestWebClientCreateRequestOption> {

		public TestWebClientCreateRequest(TestWebClientCreateRequestOption option) : base(option) { }

		public async Task<int>? RunUtility(TestHttpClientProxy testProxy) {
			if (Options.LogRawData) {
				testProxy.UseTextWriter(Console.Out);
			}
			object result = null;
			switch (Options.TestId) {
				case 0:
					result = await testProxy.GetStringResponse("test 0");
					break;
				case 1:
					result = await testProxy.GetTextResponse("test 1");
					break;
				case 2:
					result = await testProxy.GetJsonResponse("test 2");
					break;
				case 3:
					try {
						result = await testProxy.StandardError("test 3");
					}catch(Exception err) {
						result = err.ToString();
					}
					break;
				case 4:
					try {
						result = await testProxy.CustomError("test 4");
					} catch (Exception err) {
						result = err.ToString();
					}
					break;
				case 5:
					try {
						result = await testProxy.NoContent1("test 5");
					} catch (Exception err) {
						result = err.ToString();
					}
					break;
				case 6:
					try {
						result = await testProxy.NoContent2("test 6");
					} catch (Exception err) {
						result = err.ToString();
					}
					break;
			}
			Options.WriteOutput(result ?? new object());
			return 0;
		}
	}
}
