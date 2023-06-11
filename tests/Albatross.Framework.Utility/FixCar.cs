using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	public class FixCarBaseOption : BaseOption {
		[Option('m', "model", Required = true)]
		public string Model { get; set; } = null!;

		[Option('y', "year", Required = true)]
		public int Year { get; set; }
	}

	[Verb("fix-car")]
	public class FixCarOption : FixCarBaseOption {
		[Option('c', "category", Required = true)]
		public string Category { get; set; } = null!;

		[Option('e', "estimate", Required = false)]
		public int? EstimatedCost { get; set; }
	}

	public class FixCarBaseUtility {
		private readonly ILogger logger;

		public FixCarBaseUtility(ILogger logger) {
			this.logger = logger;
		}
		public Task<int> Fix(FixCarOption options) {
			logger.LogInformation("{@option}", options);
			return Task.FromResult(0);
		}
	}
	[Verb("fix-car")]
	public class FixCar : UtilityBase<FixCarOption> {
		public FixCar(FixCarOption option) : base(option) {
		}
		public Task<int> RunUtility(FixCarBaseUtility utility) {
			return utility.Fix(this.Options);
		}
	}
	[Verb("oil-change")]
	public class OilChangeOption : FixCarBaseOption { }
	public class OilChange : UtilityBase<OilChangeOption> {
		public OilChange(OilChangeOption option) : base(option) {
		}
		public Task<int> RunUtility(FixCarBaseUtility utility) {
			return utility.Fix(new FixCarOption {
				Category = "oil change",
				Clipboard = Options.Clipboard,
				EstimatedCost = 80,
				LogFile = Options.LogFile,
				Model = Options.Model,
				Year = Options.Year,
			});
		}
	}
}
