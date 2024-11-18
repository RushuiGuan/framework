using NetMQ.Sockets;
using NetMQ;
using Xunit;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Test {
	public class TestCurveEncryption {
		[Fact]
		public void Run() {
			var serverPair = new NetMQCertificate();
			using var server = new RouterSocket();
			server.Options.CurveServer = true;
			server.Options.CurveCertificate = serverPair;
			server.Bind($"tcp://127.0.0.1:55367");

			var clientPair = new NetMQCertificate();
			using var client = new DealerSocket();
			// client.Options.CurveServerKey = serverPair.PublicKey;
			client.Options.CurveServerKey = NetMQCertificate.FromPublicKey(serverPair.PublicKeyZ85).PublicKey;
			client.Options.CurveCertificate = clientPair;
			client.Connect("tcp://127.0.0.1:55367");

			for (int i = 0; i < 100; i++) {
				client.SendFrame("Hello");
				var hello = server.ReceiveMultipartMessage();
				Assert.Equal("Hello", hello[1].ConvertToString());

				var msg = new NetMQMessage();
				msg.Append(hello[0]);
				msg.Append("World");
				server.SendMultipartMessage(msg);

				var world = client.ReceiveFrameString();
				Assert.Equal("World", world);
			}
		}
	}
}
