using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.UnitTest {
    public abstract class Profile { }
    public class Profile1 : Profile { }
    public class Profile2 : Profile { }
    public class Profile3 : Profile { }
    [TestFixture]
    public class CollectionRegistrationTest {

        [Test]
        public void Test_CollectionOfConcreteClass() {
            ServiceCollection svc = new ServiceCollection();

        }
    }
}
