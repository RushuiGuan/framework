using System.Reflection;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.TimeSeries.Test { 
	public class FundEquityDbSession : DbSession {
		public FundEquityDbSession(DbContextOptions<FundEquityDbSession> option) : base(option) { }
		public override Assembly[] EntityModelAssemblies => new Assembly[] { typeof(FundEquityDbSession).Assembly };
	}
}