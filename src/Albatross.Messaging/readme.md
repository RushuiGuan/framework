
### pubsub flow
* dealer client <-> router server
	* normal flow - subscription
		1. client1 -> server1 99: subscribe or unsubscribe
		1. server1 -> client1 99: ok
	* normal flow - publishing
		1. server1 -> client1 500: topic
		1. server1 -> client2 501: topic
		1. client1 -> server1 500: ack
		1. client1 -> server1 501: ack
	* server crash
		* streaming logs
			1. find any event that has not been acked and resend
			1. client keep track of id and disgard if received duplicates
	* client crash
		1. client1 -> replay at certain time
		1. server1 -> reply the logs
		1. client1 -> ack
	* network crash
		1. don't know how to handle this yet

* client
	* client -> cmd-req
* server 
	* server -> cmd-rep
	* client -> ack


request response flow:
* dealer worker -> router broker

worker request:
	connect: identity - empty - w:connect - service1 - service2
	ready: identity - empty - w:ready
	completed: identity - empty - w:completed - payload
	error: identity - empty - w:error - payload
	shutdown: identity - empty - w:shutdown
	heatbeat: identity - empty - w:heartbeat

broker response:
	request: identity - empty - w:request - payload
	shutdown: identity - empty - w:shutdown
	ack: identity - empty - w:ack