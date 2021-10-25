using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network_Learning.Scripts.Networking
{
    public delegate void SceneLoadedDelegate(Scene _scene);
    
    public class NetworkSceneManager : NetworkBehaviour
    {
        public void LoadNetworkScene(string _scene)
        {
            if (isLocalPlayer)
            {
                CmdLoadNetworkScene(_scene);
            }
        }


        [Command]
        public void CmdLoadNetworkScene(string _scene) => RpcLoadNetworkScene(_scene);

        [ClientRpc]
        private void RpcLoadNetworkScene(string _scene) => LoadScene(_scene, _loadedScene => SceneManager.SetActiveScene(_loadedScene));

        private void LoadScene(string _sceneName, SceneLoadedDelegate _onSceneLoaded = null) => StartCoroutine(LoadScene_CR(_sceneName, _onSceneLoaded));

        private IEnumerator LoadScene_CR(string _sceneName, SceneLoadedDelegate _onSceneloaded = null)
        {
            yield return SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

            _onSceneloaded?.Invoke(SceneManager.GetSceneByName(_sceneName));
        }
    }
}

