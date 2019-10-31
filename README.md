# SimpleMQ
This is an implmentation of NetMQ into a proto-type messaging system for a micro-service architecture. It uses the XPub/XSub Pattern from NetMQ to allow for a dynamic and scalable messaging system.

It is comprised of 4 pieces:<br/>
   1) Intermediary 
       - This allows for publishers and subscribers to communicate without knowing anything about the other party.
   2) Publisher
       - This pushes messages to be processed by any number of subscribers.
   3) Subscriber
       - This listens and processes messages that are sent by publishers.
   4) Event Service
       - This is a 'blind-broker' that subscribes to a topic, stores any received messages in a database and then periodically kicks up a queue to re-send messages.
       
This was made for two reasons:<br/>
    1) To further my own understanding of Event Messaging.
    2) To create a flexible proto-type for use on a project.
    
