using System.Collections.Generic;
using Albatross.Config.Core;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Albatross.Repository.UnitTest {
	public class CRMDbSession : DbSession {
		public CRMDbSession(DbContextOptions<CRMDbSession> option, IModelBuilderFactory factory) : base(option, factory.Get(typeof(CRMDbSession).Assembly)) { }
	}
}