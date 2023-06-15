### eventing flow
Eventing flow implements a pubsub model where a topic can be published to multiple subscribers.  It offers garanteed delivery by asking the subscribers to ack each event.  

#### subscription
1. subscriber send 
subscriber -> publisher: sub
publisher -> subscriber: sub-rep


#### event publishing
publisher -> subscriber: eve
subscriber -> publisher: eve-ack