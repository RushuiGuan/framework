using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using NetMQ;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("generate-encryption-keys", typeof(GenerateEncryptionKeys))]
	public class GenerateEncryptionKeysOptions {
	}
	public class GenerateEncryptionKeys : BaseHandler<GenerateEncryptionKeysOptions> {
		public GenerateEncryptionKeys(IOptions<GenerateEncryptionKeysOptions> options) : base(options) {
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var pair = new NetMQCertificate();
			await writer.WriteLineAsync($"Public key:{pair.PublicKeyZ85}");
			await writer.WriteLineAsync($"Private key:{pair.SecretKeyZ85}");
			return 0;
		}
	}
}
