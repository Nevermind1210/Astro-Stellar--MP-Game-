using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using kcp2k;
using Mirror;
using Network_Learning.Scripts.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

namespace Network_Learning.Scripts
{
    public class ConnectionMenu : MonoBehaviour
    {
        private CustomNetworkManager networkManager;
        private KcpTransport transport;

        [SerializeField] private Button hostButton;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button connectButton;

        [Space] [SerializeField] private DiscoveredGame discoveredGameTemplate;

        private Dictionary<IPAddress, DiscoveredGame> discoveredGame = new Dictionary<IPAddress, DiscoveredGame>();

        private void Awake()
        {
            networkManager = CustomNetworkManager.instance;
            
            hostButton.onClick.AddListener(OnClickHost);
            _inputField.onEndEdit.AddListener(OnEndEditAddress);
            connectButton.onClick.AddListener(OnClickConnect);

            CustomNetworkDiscovery discovery = networkManager.discovery;
            discovery.onServerFound.AddListener(OnFoundServer);
            discovery.StartDiscovery();
        }
        

        private void OnClickHost() => networkManager.StartHost();
        private void OnEndEditAddress(string _value) => networkManager.networkAddress = _value;

        private void OnClickConnect()
        {
            string address = _inputField.text;
            ushort port = 5555;
            if (address.Contains(":"))
            {
                string portID = address.Substring(address.IndexOf(":", StringComparison.Ordinal) + 1);
                port = ushort.Parse(portID);
                address = address.Substring(0, address.IndexOf(":", StringComparison.Ordinal));
            }
            
            
            if (!IPAddress.TryParse(address, out IPAddress ipAddress))
            {
                Debug.LogError($"Invalid IP: {address}");
                address = "localhost";
            }
            
            transport.Port = port;
            networkManager.networkAddress = address;
            networkManager.StartClient();
        }
        
        private void OnFoundServer(DiscoveryResponse _response)
        {
            // Have recieved a server that is broadcasting on the network that we haven't already found
            if (!discoveredGame.ContainsKey(_response.EndPoint.Address))
            {
                // We haven't found this game already, so make the gameObject
                DiscoveredGame game = Instantiate(discoveredGameTemplate, discoveredGameTemplate.transform.parent);
                game.gameObject.SetActive(true);

                game.Setup(_response, networkManager, transport);
                discoveredGame.Add(_response.EndPoint.Address, game);
            }
        }
    }
}
