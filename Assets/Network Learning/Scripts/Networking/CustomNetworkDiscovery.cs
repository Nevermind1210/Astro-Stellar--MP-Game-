using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.Events;

namespace Network_Learning.Scripts.Networking
{
    public class DiscoveryRequest : NetworkMessage
    {
        
    }

    public class DiscoveryResponse : NetworkMessage
    {
        //The server that sent the message
        // This is a property so that it is not serialized but the client
        // fills this up after we recieve it
        public IPEndPoint EndPoint { get; set; }

        public Uri uri;

        public long serverId;
    }
 
    [Serializable] public class ServerFoundEvent : UnityEvent<DiscoveryResponse> { }
    
    public class CustomNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest,DiscoveryResponse>
    {
        #region Server

        public long serverID { get; private set; }

        [Tooltip("Transport to be advertised during discovery")]
        public Transport transport;

        [Tooltip("Invoked when a server is found")]
        public ServerFoundEvent onServerFound = new ServerFoundEvent();
        public override void Start()
        {
            serverID = RandomLong();
            
            // If the transport wasn't set in the inspector, use the active one
            // Transport.activeTransport is set in Awake of transport components
            if(transport == null)
                transport = Transport.activeTransport;
            
            base.Start();
        }

        /// <summary>
        /// Process the request from a client
        /// </summary>
        /// <remarks>
        /// Override if you wish to provide more information to the clients
        /// such as the name of the host player
        /// </remarks>
        /// <param name="request">Request coming from client</param>
        /// <param name="endPoint">Address of the client that sent the request</param>
        /// <returns>The message to be sent back to the client or null</returns>
        protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endPoint)
        {
            try
            {
                // This is just an example reply message, you could add anything h ere about the match.
                // Ie game name, game mode, game time WHATEVER!
                // This has a chance to throw an exception if the transport doesn't support network discovery.
                return new DiscoveryResponse()
                {
                    serverId = serverID,
                    uri = transport.ServerUri()
                };
            }
            catch (NotImplementedException e)
            {
                Debug.LogException(e, gameObject);
                throw;
            }
        }

        
        DiscoveryRequest GetRequest() => new DiscoveryRequest();
        #endregion

        #region Client

        protected override void ProcessResponse(DiscoveryResponse _response, IPEndPoint _endPoint)
        {
        // James is dumb.
            #region WTF

                _response.EndPoint = _endPoint;
                
                // Although we got a supposedly valid url we may not be able to resolve
                // the provided host
                // However we know the real IP address of the server because we just recieved a 
                // packet from it, so use that as a host uri
                UriBuilder realUri = new UriBuilder(_response.uri)
                {
                    Host = _response.EndPoint.Address.ToString()
                };
                _response.uri = realUri.Uri;
            
            #endregion
            
            onServerFound.Invoke(_response);
        }

        #endregion
    }
}