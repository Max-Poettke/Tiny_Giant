using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

public class NewBow : MonoBehaviour
{
    public NetworkBow networkBow;
    public NetworkObject arrows;
    public int maxArrows;
    public List<NetworkObject> arrowList = new List<NetworkObject>();
    public void OnShoot(InputAction.CallbackContext context){
        var newArrow = networkBow.SpawnArrow(arrows);
        if (context.started){
            if (arrowList.Count < maxArrows){
                arrowList.Add(newArrow);
            }
        } if (context.canceled){
            if (arrowList.Count > 0){
                networkBow.DestroyArrow(newArrow);
                arrowList.Remove(newArrow);
            }
        }
    }
}
