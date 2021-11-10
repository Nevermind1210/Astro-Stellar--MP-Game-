using System;
using System.Net;
using kcp2k;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

namespace Network_Learning.Scripts
{
    public class ConnectionMenu : MonoBehaviour
    {
        private NetworkManager _networkManager;

        [SerializeField] private Button hostButton;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button connectButton;
        private void Awake()
        {
            _networkManager = NetworkManager.singleton;
            
            hostButton.onClick.AddListener(OnClickHost);
            _inputField.onEndEdit.AddListener(OnEndEditAddress);
            connectButton.onClick.AddListener(OnClickConnect);
        }

        private void OnClickHost() => _networkManager.StartHost();
        private void OnEndEditAddress(string _value) => _networkManager.networkAddress = _value;

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
            
            ((KcpTransport)Transport.activeTransport).Port = port;
            _networkManager.networkAddress = address;
            _networkManager.StartClient();
        }
    }
}
