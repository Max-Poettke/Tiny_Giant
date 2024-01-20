using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class SpawnNetworkObjects : NetworkBehaviour
{
    //The Idea of this class is to get a list of all the objects in the scene that should be synced across Clients
    //and spawn them via the Fusion Runner.Spawn Method AFTER deleting the local version of them
    //so that their positions and movement are synchronized across clients
    
    //Get the list of objects in the scene with a NetworkObject script and put em in a list
    private List<GameObject> oldObjects = new List<GameObject>();
    private List<NetworkObject> _objects = new List<NetworkObject>();

    public void InitialiseScene()
    {
        FindGameObjectsInScene(SceneManager.GetActiveScene());
        ReplaceObjectsWithNetworkedObjects();
    }

    private void FindGameObjectsInScene(Scene scene)
    {
        oldObjects = GameObject.FindGameObjectsWithTag("SpawnNetworkObjects").ToList();
    }

    private void ReplaceObjectsWithNetworkedObjects()
    {
        //Check if the current User is the Host, if not, return
        if (!HasStateAuthority) return;
        //For each Object in the scene, spawn a version in the Network and delete the old object
        foreach (GameObject obj in oldObjects)
        {
            Debug.Log("trying to replace object: " + obj.name);
            obj.AddComponent<NetworkObject>();
            var newObj = Runner.Spawn(obj, obj.transform.position, obj.transform.rotation, Object.InputAuthority);
            newObj.AddComponent<NetworkTransform>();
            _objects.Add(newObj);
            Destroy(obj);
            Debug.Log("Destroyed object: " + obj.name);
        }
    }
}
