using Albatross.CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.EFCore.Models;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("data2", typeof(SetData2))]
	public class SetData2Option {
		[Option("n")]
		public string Name { get; set; } = string.Empty;

		[Option("p", "property-name")]
		public string? Property { get; set; }
	}
	public class SetData2 : BaseHandler<SetData2Option> {
		private readonly SampleDbSession session;

		public SetData2(SampleDbSession session, IOptions<SetData2Option> options) : base(options) {
			this.session = session;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var model = await session.DbContext.Set<Data2>().Where(x => x.Name == options.Name).FirstOrDefaultAsync();
			if (model == null) {
				model = new Data2(options.Name);
				session.DbContext.Add(model);
			}
			if (options.Property == null) {
				model.Property = null;
			} else {
				model.Property = new JsonProperty(options.Property);
			}
			await session.DbContext.SaveChangesAsync();
			return 0;
		}
	}
}