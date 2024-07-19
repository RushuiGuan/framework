using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("data2")]
	public class SetData2Option : BaseOption {
		[Option('n', Required = true)]
		public string Name { get; set; } = string.Empty;

		[Option('p', "property-name")]
		public string? Property { get; set; }
	}
	public class SetData2 : MyUtilityBase<SetData2Option> {
		public SetData2(SetData2Option option) : base(option) {
		}

		public async Task<int> RunUtility(SampleDbSession session) {
			var model = await session.DbContext.Set<Data2>().Where(x => x.Name == Options.Name).FirstOrDefaultAsync();
			if (model == null) {
				model = new Data2(Options.Name);
				session.DbContext.Add(model);
			}
			if (Options.Property == null) {
				model.Property = null;
			} else {
				model.Property = new JsonProperty(Options.Property);
			}
			await session.DbContext.SaveChangesAsync();
			return 0;
		}
	}
}
