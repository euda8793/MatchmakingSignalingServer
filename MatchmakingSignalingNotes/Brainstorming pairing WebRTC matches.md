1. **User A** requests to host a *GameSession* on **System** by providing a unique *PlayerName* and unique *GameSessionName*.
2. **System** validates both of these names are unique, and if successful creates the corresponding *PlayerHost* and *GameSession* in which its contained.
3. **User B** is able to retrieve from the **System** a list of *GameSessions* represented by their unique *GameSessionName*, which in this case would include **User A's** hosted *GameSession*.
4. Referencing **User A's** *GameSessionName* out of the retrieved list, **User B** can request to join **User A's** *GameSession* as a *PlayerClient*.
	- *GameSessions* are limited to 7 *PlayerClients* and 1 *PlayerHost*.
5. Each *Player* involved in a *GameSession* will have their *WebRTCPeerConnection* information temporarily persisted in the **System** for the duration of the *Handshake* process. 
	- The *Handshake* process entails the following (based on [WebRTCPeerConnection](https://docs.godotengine.org/en/stable/classes/class_webrtcpeerconnection.html#class-webrtcpeerconnection)):
		1.  **User A** is tied to a *PlayerHost* and **User B** is tied to a *PlayerClient* in a *GameSession*.
			- **Users** are tied to their corresponding *Player\** object via their unique *PlayerName*
			- When a *PlayerHost* or *PlayerClient* are created, their *ConnectionState* are set to *Handshake*
		1. **User A** generates and presents **System** an *Offer* using their local WebRTC client.
			- The *Offer* is persisted within a *SignalingStep* that links **User A's** *PlayerHost* to **User B's** *PlayerClient*.
		2. **User B** polls the **System** periodically for the current *SignalingStep* pertaining to **User A's** *PlayerHost*, thus retrieving the *Offer* placed by **User A's** *PlayerHost*.
			- **User B** is responsible for validating the response from **System** to ensure its *SignalingState* aligns and can request a *SignalingReset* if there is a *SignalingMismatch*.
		3. **User B** accepts the *Offer*, then provides the **System** an *Answer*.
			- The *Answer* is persisted the same way as the the *Offer*, within a *SignalingStep*.
		4. User B verifies connectivity then notifies the **System** to update its corresponding *PlayerClient* *ConnectionStatusTime* to the current time. 
		5. **User A** polls the **Service** periodically for the current *SignalingStep* pertaining to **User B's** *PlayerClient*, and thus receives the *Answer* placed by **User B's** *PlayerClient*.
			- Since **User A** is the *PlayerHost*, this step is not limited to just **User B's** *PlayerClient*, and multiple *SignalingSteps* from other *PlayerClients* could show up in the result set retrieved from polling.
		6. **User A** accepts the *Answer*, then notifies the **System** to updates its corresponding *PlayerHost*'s *ConnectionStatusTime* to the current time.
6. Every joined *PlayerClient* will perform a *Handshake* with the *PlayerHost*.
10. **User B** can periodically poll the **System** to see if it is still within the *GameSession*. 
11. **User B** can tell the **System** it would like to be removed from its current *GameSession*, in which the **System** will remove it.
	- *PlayerClients* can only exist on 1 *GameSession* at a time. 
12. **User A** can tell the **System** it would like to disband the *GameSession* in which all *PlayerClients*, *PlayerHost*, and *GameSession* will be removed.
7. All **Users** must give the **System** a periodic *ConnectionStatusUpdateTime* or they will be considered *Disconnected* upon polling by fellow *Player\** **Users**
7. **User A** can periodically poll the **System** to see if all of its *PlayerClients*' *ConnectionStatusUpdateTime* is within the *DisconnectTimeThreshold*.
8. **User B** can periodically poll the **System** to see if the *PlayerHost* or *GameSession* still exist.
9. Upon any **User** polling the **System**, any *PlayerClient* that is not within the *DisconnectTimeThreshold* will be considered *Disconnected* and its *ConnectionState* will be set to *Handshake*, so the next time **User A** (if it wasn't the one that polled) polls the **System**, the **System** will notify another *Handshake* is needed.  