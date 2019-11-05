using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Albatross.Host.NUnit {
	public abstract class TestBase<T> where T : TestScope{
		protected T CreateUnitOfWork() {

		}
    }
}