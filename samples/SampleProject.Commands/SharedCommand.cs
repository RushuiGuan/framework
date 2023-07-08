using Albatross.Messaging.Commands;
using System.Text.Json.Serialization;

namespace SampleProject.Commands {
	[JsonDerivedType(typeof(MyCommand), typeDiscriminator: "my")]
	[Command]
	public class MyCommand : BaseCommand {
		public MyCommand(int id, string name) : base(id) {
			Name = name;
		}

		public string Name { get; }
	}
	[JsonDerivedType(typeof(MyCommand), typeDiscriminator: "my")]
	[JsonPolymorphic]
	public class BaseCommand {
		public int Id { get; set; }
		public BaseCommand(int id) {
			this.Id = id;
		}
	}
}