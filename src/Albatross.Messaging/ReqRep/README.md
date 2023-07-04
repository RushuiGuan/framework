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