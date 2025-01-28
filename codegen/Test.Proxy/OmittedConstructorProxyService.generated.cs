using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class OmittedConstructorProxyService : ClientBase {
		public const string ControllerPath = "api/omittedconstructor";
	}
}
#nullable disable

