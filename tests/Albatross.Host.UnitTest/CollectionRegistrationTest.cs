using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Host.UnitTest {
	public abstract class Profile { }
    public class Profile1 : Profile { }
    public class Profile2 : Profile { }
    public class Profile3 : Profile { }
    public class CollectionRegistrationTest {

        [Fact]
        public void Test_CollectionOfConcreteClass() {
            ServiceCollection svc = new ServiceCollection();
        }
    }
}
