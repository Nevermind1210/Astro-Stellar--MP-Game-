using System;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Network_Learning.Scripts.Networking
{
    [RequireComponent(typeof(PlayerController))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private GameObject enemytoSpawn;
        [SyncVar(hook = nameof(SetColor)), SerializeField] private Color cubeColor;

        [SerializeField] private SyncList<float> syncedFloats = new SyncList<float>();

        // SyncVarHooks get called in the order the VARIABLES are defined not the functions
        // [SyncVar(hook = "SetX")] public float x;
        // [SyncVar(hook = "SetX")] public float y;
        // [SyncVar(hook = "SetX")] public float z;
        //
        // [Command]
        // public void CmdSetPosition(float _x, float _y, float _z)
        // {
        //     z = _z;
        //     x = _x;
        //     y = _y;
        // }
        
        private Material cachedMaterial;
        private void SetColor(Color _old, Color _new)
        {
            if (cachedMaterial == null)
                cachedMaterial = gameObject.GetComponent<MeshRenderer>().material;

            cachedMaterial.color = _new;
        }
        
        private void Awake()
        {
            // This will run REGARDLESS if we are localPlayer or not
            // NetworkIdentity.spawned[netId];
            Debug.Log("You are valid and beautiful!");
        }

        private void Update()
        {
            MeshRenderer render = gameObject.GetComponent<MeshRenderer>();
            render.material.color = cubeColor;
            // Determine if player is Local player.
            if (isLocalPlayer)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Run function that tells every client to change the colour of this gameObject
                    CmdRandomColor();
                }

                if (Input.GetKeyDown(KeyCode.N))
                {
                    OnStartServer();
                }
            }
        }

        // RULES FOR COMMANDS:
        // 1. Cannot return anything
        // 2. Must follow the correct naming convention: The function MUST started with "Cmd" exactly like that
        // 3. The function must have the attribute [Command] found in Mirror namespace
        // 4. Can only be certain serializable types (See Command in the documentation)
        [Command]
        public void CmdRandomColor()
        {
            // This is running on the server.
            RpcRandomColor(Random.Range(0f, 1f));
        }

        [Command]
        public void CmdSpawnEnemy()
        {
            // NetworkServer.spawn requires an instance of the object in the server's scene to be present
            // So if the object being spawned isa prefab, instantiate needs to be called first
            GameObject newEnemy = Instantiate(enemytoSpawn);
            NetworkServer.Spawn(newEnemy);
        }
        
        // RULES FOR CLIENT RPC:
        // 1. Cannot return anything
        // 2. Must follow the correct naming convention: The function MUST started with "Rpc" exactly like that
        // 3. The function must have the attribute [ClientRpc] found in Mirror namespace
        // 4. Can only be certain serializable types (See Command in the documentation)
        [ClientRpc]
        public void RpcRandomColor(float _hue)
        {
            // This is running on every instance of the same object that the client was calling from.
            // I.e. Red GO on Red Client runs Cmd, Red GO on Red, Green and Blue client's run Rpc.
            MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
            rend.material.color = Color.HSVToRGB(_hue, 1, 1);
        }
        
        
        // This is run via the network starting and player connecting...
        // NOT Unity
        // It's is run when the object is spawned via the networking system NOT when Unity
        // Instantiates the object.
        public override void OnStartLocalPlayer()
        {
            // This runs if we are  the local player and NOT a remote player
        }

        // This is run via the network starting the player connecting...
        // NOT unity
        // It is run when the object is spawned via the networking system NOT when Unity
        // Instantiates the object
        public override void OnStartClient()
        {
            // This runs REGARDLESS of local or not.
            
            // islocalPlayer is true if this object is the client's local player otherwise it's false
            
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            controller.enabled = isLocalPlayer;
            
            //CustomNetworkManager.AddPlayer(this);
        }

        // This runs when the server starts... ON the server
        // In the case of a Host-Client situation, this only runs when the HOST launches because the host is the server
        public override void OnStartServer()
        {
            for (int i = 0; i < 10; i++) 
                syncedFloats.Add(Random.Range(0, 11));
        }
        
    }
}