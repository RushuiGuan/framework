### command flow:
Command flow is a pattern that can garantee the order of execution as well as the execution itself.  The patterns uses queues to manage the order of execution.  Requests within the same queue will be executed sequentially.  It has two parties: the command client and command bus.  Client sends requests and the command bus manages execution queues and execute the requests.  Even through the queues are managed in the command bus but they are created at the requested of the client.  The implementation garantees execution by using durable queues.  It means that all messages to and from the command bus are logged.  In the event of crash, the command bus can resume from the last execution.

The pattern has two flows:

#### Command flow1: Fire and Forget
1. command client send a request (cmd-req)
1. command bus acknowledge (cmd-req-ack)
1. command bus queue and execute the request
	1. the request is garantee to execute (success or fail)

#### Use Case 2 - Fire and callback
1. command client send a request (cmd-req)
1. command bus queue and execute the request
1. command bus send back the result (cmd-rep or cmd-rep-err)
1. client ack (ack)

The preferred workflow is fire and forget.  Fire and callback flow can behavor like fire and forget if the consumer simply ignore the response.  But the lack of request ack means that there is a chance that the server didn't receive the request in the first place.  Fire and forget flow allows a command to be submitted from within another command handler.  It is not something that Fire and callback can do because it will cause deadlocks if the caller choose to wait for the callback.