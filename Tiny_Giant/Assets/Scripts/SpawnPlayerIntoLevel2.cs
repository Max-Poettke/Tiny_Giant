using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class SpawnPlayerIntoLevel2 : NetworkBehaviour
{
   private GameObject _player;
   private GameObject _vrPlayer;
   private CinemachineStoryboard _cine;
   private float fadeTime = 1f;

   public override void Spawned()
   {
      _player = GameObject.FindWithTag("Player");
      if (!_player) return;
      _cine = _player.GetComponentInChildren<CinemachineStoryboard>();
      _player.GetComponentInChildren<NetworkRigidbody3D>().Teleport(transform.position);
      StartCoroutine(FadeBack());

      _vrPlayer = GameObject.FindWithTag("VRPlayer");
      _vrPlayer.GetComponent<XRScrollMap>().enabled = false;
      if (!_vrPlayer) return;
      _vrPlayer.transform.position = new Vector3(15f, -60f, 0f);
   }

   private IEnumerator FadeBack()
   {
      yield return new WaitForSeconds(2f);
      var time = 0f;
      while (time < fadeTime)
      {
         _cine.m_Alpha = Mathf.Lerp(1f, 0f, time / fadeTime);
         time += Time.deltaTime;
         yield return null;
      }

      _player.GetComponent<FirstPersonController>().playerCanMove = true;
      _cine.m_Alpha = 0f;
   }
}
