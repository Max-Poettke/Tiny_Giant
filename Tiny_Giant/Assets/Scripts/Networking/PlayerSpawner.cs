using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefabPC;
    public GameObject playerPrefabVR;
    public GameObject pcSpawnPoint;
    public GameObject vrSpawnPoint;

    public void PlayerJoined(PlayerRef player)
    {
        /*
        if (player == Runner.LocalPlayer)
        {
            var pcPlayer = Runner.Spawn(playerPrefabPC, pcSpawnPoint.transform.position, Quaternion.identity, player);
        }
        */
        
        
        
        
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefabVR, vrSpawnPoint.transform.position, Quaternion.identity, player);
        }
        
        
        
 
    }
}
