using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefabPC;
    public GameObject playerPrefabVR;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
        {
            Runner.Spawn(playerPrefabPC, new Vector3(0, 1, 0), Quaternion.identity, player);
        }

        if (player == Runner.LocalPlayer && (Application.platform == RuntimePlatform.Android))
        {
            Runner.Spawn(playerPrefabVR, new Vector3(0, 1, 1), Quaternion.identity, player);
        }
    }
}
