using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefabPC;
    [SerializeField] private NetworkPrefabRef _playerPrefabVR;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            NetworkObject networkPlayerObject;
            Debug.Log(player.PlayerId);
            if (player.PlayerId == 1)
            {
                Vector3 spawnPos = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
                networkPlayerObject = runner.Spawn(_playerPrefabPC, spawnPos, Quaternion.identity, player);
                spawnedCharacters.Add(player, networkPlayerObject);    
            }
            else
            {
                Vector3 spawnPos = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
                networkPlayerObject = runner.Spawn(_playerPrefabVR, spawnPos, Quaternion.identity, player);
                spawnedCharacters.Add(player, networkPlayerObject);
                Debug.Log("Deactivated Camera");
                networkPlayerObject.GetComponentInChildren<Camera>().enabled = false;
            }
            
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkPlayerObject))
        {
            runner.Despawn(networkPlayerObject);
            spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    private NetworkRunner _runner;
    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if(GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}
