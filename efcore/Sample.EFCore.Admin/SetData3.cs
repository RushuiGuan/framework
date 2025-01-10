using Albatross.CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.EFCore.Models;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("data3", typeof(SetData3))]
	public class SetData3Option {
		[Option("n")]
		public string Name { get; set; } = string.Empty;

		[Option("p")]
		public string? Property { get; set; }

		[Option("c")]
		public bool Clear { get; set; }
	}
	public class SetData3 : BaseHandler<SetData3Option> {
		private readonly SampleDbSession session;

		public SetData3(SampleDbSession session, IOptions<SetData3Option> options) : base(options) {
			this.session = session;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var model = await session.DbContext.Set<Data3>().Where(x => x.Name == options.Name).FirstOrDefaultAsync();
			if (model == null) {
				model = new Data3(options.Name);
				session.DbContext.Add(model);
			}
			if (options.Clear) {
				model.ArrayProperty.Clear();
			} else {
				model.ArrayProperty.Add(new JsonProperty(options.Property));
			}
			await session.DbContext.SaveChangesAsync();
			return 0;
		}
	}
}