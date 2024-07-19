using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("data3")]
	public class SetData3Option : BaseOption {
		[Option('n', Required = true)]
		public string Name { get; set; } = string.Empty;

		[Option('p')]
		public string? Property { get; set; }

		[Option('c')]
		public bool Clear { get; set; }
	}
	public class SetData3 : MyUtilityBase<SetData3Option> {
		public SetData3(SetData3Option option) : base(option) {
		}

		public async Task<int> RunUtility(SampleDbSession session) {
			var model = await session.DbContext.Set<Data3>().Where(x => x.Name == Options.Name).FirstOrDefaultAsync();
			if (model == null) {
				model = new Data3(Options.Name);
				session.DbContext.Add(model);
			}
			if(Options.Clear) {
				model.ArrayProperty.Clear();
			} else {
				model.ArrayProperty.Add(new JsonProperty(Options.Property));
			}
			await session.DbContext.SaveChangesAsync();
			return 0;
		}
	}
}
