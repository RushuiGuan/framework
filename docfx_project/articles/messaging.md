# Albatross.Messaging
The messaging assembly uses ZeroMQ as the transport layer.  It currently supports two messaging patterns: command queue and pub sub.  It supports sequential processing(of commands), out of order connect and durable queues.

The messaging dll can be used to create a daemon service that represent the "C" part of the CQSR pattern.  Its command queue implementation allows:
* Dynamic queue creation
* Sequential command processing within the same queue
* Durable queues - messages are saved to disk when received and and be resumed when the service is restarted.
* out of order connection - the "Q" (Query) part of the CQRS pattern often comes from a web service.  The web service is also used to forward commands to the daemon service therefore can be considered as a consumer\client of the daemon service.  Out of order connection means that the client and the server can be started out of sequence.  They will connect with each other as long as they are both up and running.  This feature makes it much easier to manage service infrastruture.

Under the hood, the dll uses the ZeroMQ's router socket for servers (daemon service) and its dealer socket for the clients (consumer, client).  When connected, the client will maintane a constant tcp\ip connection with the server.  The the server supports multiple clients but there is a cost to connect and disconnect and the it is more efficient to have long lived clients.  

## How to Create a new Messaging Daemon
Please find the messaging daemon sample code here [sample-daemon]()

## Use of `Albatross.Messaging.CodeGen`
Since creating short lived clients is not the most efficient way to talk to the messaging daemon.  It is recommended to communicate via a long live client such as web api.  For example, we can have 

```
	command line utility (short lived)
		<-- httpclient --> web api 
		<-- zeromq --> messaging daemon
```  


By doing so, the daemon utility can also be walled off and access control can be implemented by the web api.  `Albatross.Messaging.CodeGen` is created to simplify json serialization and deserialization of the commands between the web api and the daemon.  To use it: 

1. A web api project `Sample.Messaging.WebApi` is created and configured as the messaging service consumer.
1. A daemon service `Sample.Messaging.Daemon` with messaging is created as the messaging host.
1. A command line utility `Sample.Messaging.Utility` is created to send commands to the web api.
1. A dto project `Sample.Messaging.Core` is created to share the command dtos between the daemin, web api and the utility.
1. In the dto project, create a partial public interface that is prefixed by `I` and postfixed by `Command`.  The interface should be empty.
```c#
public partial interface ITestCommand { }
```
1. Reference the `Albatross.Messaging.CodeGen` dll and make sure the codegen is referenced with the following properties:
```xml
<PackageReference Include="Albatross.Messaging.CodeGen" Version="xxx">
	<PrivateAssets>all</PrivateAssets>
	<ReferenceOutputAssembly>false</ReferenceOutputAssembly>
	<OutputItemType>Analyzer</OutputItemType>
</PackageReference>
```
1. It might help to set the project property `EmitCompilerGeneratedFiles` to true so that the generated file is visible.
  ```xml
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  ```
1. When creating a new command, have it implement the said interface `ITestCommand`.  
```c#
public record class MyTestCommand : ITestCommand {
	public MyTestCommand(string parameter) {
		this.Parameter = parameter;
	}
	public string Parameter { get; }
}
```
The code generator will auto generate the `JsonDerivedTypeAttribute` on the interface so that only the interface is needed to create the web api endpoint for submitting the commands.
```c#
[JsonDerivedType(typeof(MyTestCommand), "MyTestCommand")]
public partial interface ITestCommand
{
}
```

